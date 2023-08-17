import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { useParams } from 'react-router-dom';
import { Container, Grid } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import Loading from '../../components/Loading';
import EmptyBlock from '../../components/EmptyBlock';
import ProfileHeader from './ProfileHeader';
import ProfileContent from './ProfileContent';

export default observer(function ProfilePage() {
  const { username } = useParams();
  const { profileStore } = useStore();

  const { loadingProfile, loadProfile, profile } = profileStore;

  useEffect(() => {
    if (username) {
      loadProfile(username);
    }
  }, [username, loadProfile]);

  if (loadingProfile) {
    return <Loading content="Loading profile..." />;
  }

  if (!profile) {
    return <EmptyBlock text="The profile is empty" />;
  }

  return (
    <Container>
      <Grid>
        <Grid.Column width={16}>
          <ProfileHeader profile={profile} />
          <ProfileContent profile={profile} />
        </Grid.Column>
      </Grid>
    </Container>
  );
});