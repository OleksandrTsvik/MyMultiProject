import { observer } from 'mobx-react-lite';
import { Button } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import BirthdayModalForm, { BirthdayForm } from './BirthdayModalForm';

export default observer(function AddBirthdayButton() {
  const { modalStore, birthdayStore } = useStore();

  const { openModal } = modalStore;
  const { createBirthday } = birthdayStore;

  function handleSubmit(createdBirthday: BirthdayForm) {
    return createBirthday(createdBirthday);
  }

  return (
    <Button
      className="m-0"
      icon="add"
      color="green"
      onClick={() =>
        openModal(
          <BirthdayModalForm
            title="Add a new birthday"
            textForSubmitBtn="Add"
            initialValues={{
              fullName: '',
              date: new Date(),
            }}
            onSubmit={handleSubmit}
          />,
          {},
          true,
        )
      }
    />
  );
});
