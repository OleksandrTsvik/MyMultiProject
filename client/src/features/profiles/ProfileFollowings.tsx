import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';
import { Card, Grid, Header, Icon, Image, Tab } from 'semantic-ui-react';

import { ListFollowingsPredicate } from '../../app/models/profile';
import { useStore } from '../../app/stores/store';
import EmptyBlock from '../../components/EmptyBlock';
import FollowButton from './FollowButton';

interface Props {
    predicate: ListFollowingsPredicate;
}

export default observer(function ProfileFollowings({ predicate }: Props) {
    const { profileStore } = useStore();
    const {
        profile, followings,
        loadFollowings, loadingFollowings,
        resetFollowings
    } = profileStore;

    useEffect(() => {
        loadFollowings(predicate);
    }, [loadFollowings, predicate]);

    useEffect(() => {
        return () => {
            resetFollowings();
        };
    }, [resetFollowings]);

    return (
        <Tab.Pane loading={loadingFollowings}>
            <Grid>
                <Grid.Column width={16}>
                    <Header
                        icon="users"
                        floated="left"
                        content={predicate === ListFollowingsPredicate.followers
                            ? `People following ${profile?.userName}`
                            : `People ${profile?.userName} is following`
                        }
                    />
                </Grid.Column>
                <Grid.Column width={16}>
                    <Card.Group
                        stackable
                        doubling
                        itemsPerRow={3}
                    >
                        {followings.length === 0
                            ? <EmptyBlock className="mx-3" />
                            : followings.map((profile) => (
                                <Card
                                    key={profile.userName}
                                    as={Link} to={`/profiles/${profile.userName}`}
                                >
                                    <Image src={profile.image || '/assets/user.png'} />
                                    <Card.Content>
                                        <Card.Header>{profile.userName}</Card.Header>
                                    </Card.Content>
                                    <Card.Content extra>
                                        <Icon name='user' />
                                        {profile.followersCount} followers
                                    </Card.Content>
                                    <Card.Content extra>
                                        <FollowButton profile={profile} />
                                    </Card.Content>
                                </Card>
                            ))
                        }
                    </Card.Group>
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    );
});