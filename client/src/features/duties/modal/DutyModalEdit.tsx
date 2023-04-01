import { ChangeEvent, useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Form, Modal } from 'semantic-ui-react';

import { Duty } from '../../../app/models/duty';
import { useStore } from '../../../app/stores/store';

export default observer(function DutyModalEdit() {
    const { dutyStore } = useStore();
    const { getIsLoading, selectedDuty, editDuty, editMode, closeEditMode } = dutyStore;

    const [duty, setDuty] = useState<Duty | undefined>(selectedDuty);

    useEffect(() => {
        setDuty(selectedDuty);
    }, [selectedDuty]);

    function handleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        if (!duty) {
            return;
        }

        const { name, value } = event.target;

        setDuty({ ...duty, [name]: value });
    }

    if (!duty) {
        return null;
    }

    return (
        <Modal
            centered={false}
            open={editMode}
            onClose={() => closeEditMode()}
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
                    onClick={() => closeEditMode()}
                >
                    Cancel
                </Button>
                <Button
                    positive
                    loading={getIsLoading(duty.id)}
                    disabled={getIsLoading(duty.id)}
                    onClick={() => editDuty(duty)}
                >
                    Update
                </Button>
            </Modal.Actions>
        </Modal>
    );
});