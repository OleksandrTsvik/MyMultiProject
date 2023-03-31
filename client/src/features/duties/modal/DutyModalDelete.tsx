import { useEffect, useState } from 'react';
import { Button, Icon, Modal } from 'semantic-ui-react';

import { Duty } from '../../../app/models/duty';
import { useStore } from '../../../app/stores/store';
import agent from '../../../app/api/agent';
import dateFormat from '../../../app/utils/dateFormat';

interface Props {
    duty: Duty;
}

interface Style {
    color: string;
    backgroundColor: string;
    borderColor: string;
}

export default function DutyModalDelete({ duty: selectedDuty }: Props) {
    const { dutyStore } = useStore();
    const { deleteMode, closeDeleteMode } = dutyStore;

    const [submitting, setSubmitting] = useState<boolean>(false);
    const [style, setStyle] = useState<Style>({ color: '#000000', backgroundColor: '#ffffff', borderColor: '#ffffff' });

    useEffect(() => {
        setStyle({
            color: selectedDuty.fontColor,
            backgroundColor: selectedDuty.backgroundColor,
            borderColor: selectedDuty.fontColor
        });
    }, [selectedDuty]);

    function handleSubmit(id: string) {
        setSubmitting(true);

        agent.Duties.delete(id).then(() => {
            setSubmitting(false);
            closeDeleteMode(selectedDuty.id);
        });
    }

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
                <p className='fw-bold'>{selectedDuty.title}</p>
            </Modal.Content>
            <hr style={style} />
            <Modal.Content style={style}>
                <p>{selectedDuty.description}</p>
            </Modal.Content>
            <hr style={style} />
            <Modal.Content className='text-end' style={style}>
                <Icon name='calendar alternate' />&ensp;
                {dateFormat(selectedDuty.dateCreation)}
            </Modal.Content>
            <Modal.Actions style={style}>
                <Button
                    negative
                    onClick={() => closeDeleteMode()}
                >
                    <Icon name='remove' /> No
                </Button>
                <Button
                    positive
                    loading={submitting}
                    onClick={() => handleSubmit(selectedDuty.id)}
                >
                    <Icon name='checkmark' /> Yes
                </Button>
            </Modal.Actions>
        </Modal>
    );
}