import { useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Card, Image } from 'semantic-ui-react';

import { Photo, Profile } from '../../app/models/profile';
import { useStore } from '../../app/stores/store';

interface Props {
    profile: Profile;
}

export default observer(function ProfilePhotosList({ profile }: Props) {
    const { profileStore } = useStore();
    const {
        isCurrentUser,
        loadingSetMain, loadingDelete,
        setMainPhoto, deletePhoto
    } = profileStore;

    const [loadingPhotoId, setLoadingPhotoId] = useState<string | null>(null);

    function handleSetMainPhoto(photo: Photo) {
        setLoadingPhotoId(photo.id);
        setMainPhoto(photo);
    }

    function handleDeletePhoto(photo: Photo) {
        setLoadingPhotoId(photo.id);
        deletePhoto(photo);
    }

    return (
        <Card.Group
            stackable
            doubling
            itemsPerRow={3}
        >
            {profile.photos.map((photo) => (
                <Card key={photo.id}>
                    <Image
                        src={photo.url}
                        label={photo.isMain
                            ? {
                                color: 'teal',
                                icon: 'star',
                                corner: 'right'
                            }
                            : undefined
                        }
                    />
                    {isCurrentUser &&
                        <Card.Content extra>
                            <Button.Group widths={2}>
                                <Button
                                    basic
                                    color="teal"
                                    content="Main"
                                    loading={loadingPhotoId === photo.id && loadingSetMain}
                                    disabled={photo.isMain || (loadingPhotoId === photo.id && (loadingSetMain || loadingDelete))}
                                    onClick={() => handleSetMainPhoto(photo)}
                                />
                                <Button
                                    basic
                                    color="red"
                                    icon="trash"
                                    loading={loadingPhotoId === photo.id && loadingDelete}
                                    disabled={loadingPhotoId === photo.id && (loadingDelete || loadingSetMain)}
                                    onClick={() => handleDeletePhoto(photo)}
                                />
                            </Button.Group>
                        </Card.Content>
                    }
                </Card>
            ))}
        </Card.Group>
    );
});