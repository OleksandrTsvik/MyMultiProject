import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Container, Header } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import Loading from '../../components/Loading';
import EmptyBlock from '../../components/EmptyBlock';
import BirthdaysList from './BirthdaysList';
import AddBirthday from './AddBirthdayButton';

export default observer(function BirthdaysPage() {
  const { birthdayStore } = useStore();
  const { loadBirthdays, loadingBirthdays, resetBirthdays, birthdays } =
    birthdayStore;

  useEffect(() => {
    loadBirthdays();

    return () => {
      resetBirthdays();
    };
  }, [loadBirthdays, resetBirthdays]);

  if (loadingBirthdays) {
    return <Loading content="Loading birthday list..." dimmer={false} />;
  }

  return (
    <Container>
      <div className="d-flex gap-3 mb-4">
        <Header
          as="h1"
          className="flex-fill mb-0"
          content="Birthdays"
          textAlign="center"
        />
        <AddBirthday />
      </div>
      {birthdays.size === 0 ? <EmptyBlock /> : <BirthdaysList />}
    </Container>
  );
});
