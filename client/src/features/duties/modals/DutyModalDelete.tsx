import { observer } from 'mobx-react-lite';
import { Button, Icon, Modal } from 'semantic-ui-react';
import { format } from 'date-fns';

import { useStore } from '../../../app/stores/store';
import { Style } from './DutyModalCreate';

export default observer(function DutyModalDelete() {
  const { dutyStore } = useStore();
  const { getIsLoading, selectedDuty, deleteDuty, deleteMode, closeDeleteMode } = dutyStore;

  if (!selectedDuty) {
    return null;
  }

  const style: Style = {
    color: selectedDuty.fontColor,
    backgroundColor: selectedDuty.backgroundColor,
    borderColor: selectedDuty.fontColor
  };

  return (
    <Modal
      centered={false}
      open={deleteMode}
      onClose={() => closeDeleteMode()}
      style={style}
    >
      <Modal.Header style={style}>Delete duty?</Modal.Header>
      <Modal.Content style={style}>
        <p>Are you sure you want to delete duty?</p>
        <p className="fw-bold">{selectedDuty.title}</p>
      </Modal.Content>
      <hr style={style} />
      <Modal.Content style={style}>
        <p>{selectedDuty.description}</p>
      </Modal.Content>
      <hr style={style} />
      <Modal.Content style={style} className="text-end">
        <Icon name="calendar alternate" />&ensp;
        {format(selectedDuty.dateCreation, "LLLL dd, iiii 'at' kk:mm:ss, y")}
      </Modal.Content>
      <Modal.Actions style={style}>
        <Button
          negative
          onClick={() => closeDeleteMode()}
        >
          <Icon name="remove" /> No
        </Button>
        <Button
          positive
          loading={getIsLoading(selectedDuty.id)}
          disabled={getIsLoading(selectedDuty.id)}
          onClick={() => deleteDuty(selectedDuty.id)}
        >
          <Icon name="checkmark" /> Yes
        </Button>
      </Modal.Actions>
    </Modal>
  );
});