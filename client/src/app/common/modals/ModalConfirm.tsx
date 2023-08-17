import { observer } from 'mobx-react-lite';
import { Button, ButtonProps, Icon, Modal, ModalActionsProps, ModalContentProps, ModalHeaderProps } from 'semantic-ui-react';

import { useStore } from '../../stores/store';

interface Props {
  content?: string;
  header?: string;
  onConfirm: () => void;
  onCancel?: () => void;
  isLoading?: boolean;
  closeAfterConfirm?: boolean;
  closeAfterCancel?: boolean;
  btnConfirmProps?: ButtonProps;
  btnCancelProps?: ButtonProps;
  headerProps?: ModalHeaderProps;
  contentProps?: ModalContentProps;
  actionsProps?: ModalActionsProps;
}

export default observer(
  function ModalConfirm(
    {
      content,
      header = 'Are you sure?',
      onConfirm,
      onCancel,
      isLoading,
      closeAfterConfirm,
      closeAfterCancel = true,
      btnConfirmProps,
      btnCancelProps,
      headerProps,
      contentProps,
      actionsProps
    }: Props
  ) {
    const { modalStore } = useStore();
    const { closeModal } = modalStore;

    function handleConfirm() {
      onConfirm && onConfirm();
      closeAfterConfirm && closeModal();
    }

    function handleCancel() {
      onCancel && onCancel();
      closeAfterCancel && closeModal();
    }

    return (
      <>
        {header && <Modal.Header {...headerProps}>{header}</Modal.Header>}
        {content && <Modal.Content {...contentProps}>{content}</Modal.Content>}
        <Modal.Actions {...actionsProps}>
          <Button
            content={<><Icon name="remove" /> Cancel</>}
            {...btnConfirmProps}
            onClick={handleCancel}
          />
          <Button
            primary
            content={<><Icon name="checkmark" /> OK</>}
            {...btnCancelProps}
            loading={isLoading}
            disabled={isLoading}
            onClick={handleConfirm}
          />
        </Modal.Actions>
      </>
    );
  }
);