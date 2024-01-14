import { observer } from 'mobx-react-lite';
import { Button } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import ModalConfirm from '../../app/common/modals/ModalConfirm';
import { Birthday } from '../../app/models/birthday';

interface Props {
  birthday: Birthday;
}

export default observer(function DeleteBirthdayButton({ birthday }: Props) {
  const { modalStore, birthdayStore } = useStore();

  const { openModal } = modalStore;
  const { deleteBirthday } = birthdayStore;

  function handleOpenModal() {
    openModal(
      <ModalConfirm
        content={<p>Delete the birthday &quot;{birthday.fullName}&quot;?</p>}
        onConfirm={() => deleteBirthday(birthday.id)}
      />,
      {},
      true,
    );
  }

  return (
    <Button
      className="m-0"
      icon="trash alternate"
      color="red"
      onClick={() => handleOpenModal()}
    />
  );
});
