import React, { ChangeEvent, useState } from 'react';
import { Button, Form, Modal } from 'semantic-ui-react';

import { Duty } from '../../../app/models/duty';
import agent from '../../../app/api/agent';

interface Props {
    duty: Duty;
    setDuties: React.Dispatch<React.SetStateAction<Duty[]>>;
    editMode: boolean;
    closeEditMode: () => void;
}

export default function DutyModalEdit({ duty: selectedDuty, setDuties, editMode, closeEditMode }: Props) {
    const [submitting, setSubmitting] = useState<boolean>(false);
    const [duty, setDuty] = useState<Duty>(selectedDuty);

    function handleSubmit() {
        setSubmitting(true);

        agent.Duties.update(duty).then(() => {
            setDuties((duties) => (
                [...duties.filter(task => task.id !== duty.id), duty]
            ));

            setSubmitting(false);
            closeEditMode();
        });
    }

    function handleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        const { name, value } = event.target;

        setDuty({ ...duty, [name]: value });
    }

    return (
        <Modal
            centered={false}
            open={editMode}
            onClose={closeEditMode}
        >
            <Modal.Header>Edit task</Modal.Header>
            <Modal.Content>
                <Form>
                    <Form.Field>
                        <Form.Input
                            label='Title'
                            placeholder='Title'
                            value={duty.title}
                            name='title'
                            onChange={handleInputChange}
                        />
                    </Form.Field>
                    <Form.Field>
                        <Form.TextArea
                            label='Description'
                            placeholder='Description'
                            value={duty.description}
                            name='description'
                            onChange={handleInputChange}
                            style={{ minHeight: 100 }}
                        />
                    </Form.Field>
                </Form>
            </Modal.Content>
            <Modal.Actions>
                <Button
                    negative
                    onClick={closeEditMode}
                >
                    Cancel
                </Button>
                <Button
                    positive
                    loading={submitting}
                    onClick={handleSubmit}
                >
                    Update
                </Button>
            </Modal.Actions>
        </Modal>
    );
}