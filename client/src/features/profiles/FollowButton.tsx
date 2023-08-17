import { SyntheticEvent } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Reveal } from 'semantic-ui-react';

import { Profile } from '../../app/models/profile';
import { useStore } from '../../app/stores/store';

interface Props {
  profile: Profile;
}

export default observer(function FollowButton({ profile }: Props) {
  const { profileStore, userStore } = useStore();

  const { updateFollowing, loadingFollow } = profileStore;
  const { user } = userStore;

  if (user === null || user.userName === profile.userName) {
    return null;
  }

  function handleFollow(event: SyntheticEvent) {
    event.preventDefault();

    if (profile.following) {
      updateFollowing(profile.userName, false);
    } else {
      updateFollowing(profile.userName, true);
    }
  }

  return (
    <Reveal animated="move">
      <Reveal.Content visible className="w-100">
        <Button
          fluid
          color="teal"
          content={profile.following ? 'Following' : 'Not following'}
        />
      </Reveal.Content>
      <Reveal.Content hidden className="w-100">
        <Button
          fluid
          basic
          loading={loadingFollow}
          disabled={loadingFollow}
          color={profile.following ? 'red' : 'green'}
          content={profile.following ? 'Unfollow' : 'Follow'}
          onClick={handleFollow}
        />
      </Reveal.Content>
    </Reveal>
  );
});