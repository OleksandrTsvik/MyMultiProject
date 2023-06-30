import { useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Grid, Header, Tab } from 'semantic-ui-react';

import { Profile } from '../../app/models/profile';
import { useStore } from '../../app/stores/store';
import PhotoUploadWidget from '../../app/common/imageUpload/PhotoUploadWidget';
import EmptyBlock from '../../components/EmptyBlock';
import ProfilePhotosList from './ProfilePhotosList';

interface Props {
    profile: Profile;
}

export default observer(function ProfilePhotos({ profile }: Props) {
    const { profileStore } = useStore();
    const { isCurrentUser, uploadPhoto, uploading } = profileStore;

    const [addPhotoMode, setAddPhotoMode] = useState<boolean>(false);

    if (!isCurrentUser && profile.photos.length === 0) {
        return <EmptyBlock text="There are no photos" />;
    }

    function handlePhotoUpload(file: Blob) {
        uploadPhoto(file)
            .then(() => setAddPhotoMode(false));
    }

    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Header
                        icon="images"
                        floated="left"
                        content="Photos"
                    />
                    {isCurrentUser &&
                        <Button
                            basic
                            floated="right"
                            loading={uploading}
                            disabled={uploading}
                            color={addPhotoMode ? 'red' : 'teal'}
                            content={addPhotoMode ? 'Cancel' : 'Add Photo'}
                            onClick={() => setAddPhotoMode(!addPhotoMode)}
                        />
                    }
                </Grid.Column>
                <Grid.Column width={16}>
                    {isCurrentUser && addPhotoMode
                        ? <PhotoUploadWidget
                            loading={uploading}
                            onPhotoUpload={handlePhotoUpload}
                        />
                        : profile.photos.length === 0
                            ? <EmptyBlock text="There are no photos" />
                            : <ProfilePhotosList profile={profile} />
                    }
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    );
});