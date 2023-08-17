import { observer } from 'mobx-react-lite';
import { Modal } from 'semantic-ui-react';

import { useStore } from '../../stores/store';

export default observer(function ModalContainer() {
  const { modalStore } = useStore();
  const { isOpen, body, props, replaceContent, closeModal } = modalStore;

  return (
    <Modal
      open={isOpen}
      onClose={closeModal}
      {...props}
    >
      {replaceContent
        ? body
        : <Modal.Content>{body}</Modal.Content>
      }
    </Modal>
  );
});