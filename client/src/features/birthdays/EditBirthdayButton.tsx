import { observer } from 'mobx-react-lite';
import { Button } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import { Birthday } from '../../app/models/birthday';
import BirthdayModalForm, { BirthdayForm } from './BirthdayModalForm';

interface Props {
  birthday: Birthday;
}

export default observer(function EditBirthdayButton({ birthday }: Props) {
  const { modalStore, birthdayStore } = useStore();

  const { openModal } = modalStore;
  const { editBirthday } = birthdayStore;

  function handleSubmit(editedBirthday: BirthdayForm) {
    return editBirthday({
      ...editedBirthday,
      id: birthday.id,
    });
  }

  return (
    <Button
      className="m-0"
      icon="pencil alternate"
      color="blue"
      onClick={() =>
        openModal(
          <BirthdayModalForm
            title="Update birthday"
            textForSubmitBtn="Update"
            initialValues={{ ...birthday, note: birthday.note ?? '' }}
            onSubmit={handleSubmit}
          />,
          {},
          true,
        )
      }
    />
  );
});
