import React, { useEffect, useState } from 'react';
import { Button, Icon, Modal } from 'semantic-ui-react';
import { Duty } from '../../../app/models/duty';

interface Props {
    duty: Duty;
    deleteMode: boolean;
    closeDeleteMode: (id?: string) => void;
}

interface Style {
    color: string;
    backgroundColor: string;
}

export default function DutyModalDelete({ duty: selectedDuty, deleteMode, closeDeleteMode }: Props) {
    const [style, setStyle] = useState<Style>({ color: '#000', backgroundColor: '#fff' });

    useEffect(() => {
        setStyle({
            color: `${selectedDuty.fontColor} !important`,
            backgroundColor: `${selectedDuty.backgroundColor} !important`
        });
    }, [selectedDuty]);

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
            <hr />
            <Modal.Content className='text-end' style={style}>
                {selectedDuty.dateCreation}
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
                    onClick={() => closeDeleteMode(selectedDuty.id)}
                >
                    <Icon name='checkmark' /> Yes
                </Button>
            </Modal.Actions>
        </Modal>
    );
}