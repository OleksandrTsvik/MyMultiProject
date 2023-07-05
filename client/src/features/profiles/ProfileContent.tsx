import { observer } from 'mobx-react-lite';
import { Tab } from 'semantic-ui-react';

import { ListFollowingsPredicate, Profile } from '../../app/models/profile';
import ProfilePhotos from './ProfilePhotos';
import ProfileFollowings from './ProfileFollowings';

interface Props {
    profile: Profile;
}

export default observer(function ProfileContent({ profile }: Props) {
    const panes = [
        { menuItem: 'About', render: () => <Tab.Pane>About Content</Tab.Pane> },
        { menuItem: 'Photos', render: () => <ProfilePhotos profile={profile} /> },
        { menuItem: 'Followers', render: () => <ProfileFollowings predicate={ListFollowingsPredicate.followers} /> },
        { menuItem: 'Following', render: () => <ProfileFollowings predicate={ListFollowingsPredicate.following} /> }
    ];

    return (
        <Tab
            menu={{
                color: 'teal',
                fluid: true,
                vertical: true
            }}
            menuPosition="right"
            panes={panes}
        />
    );
});