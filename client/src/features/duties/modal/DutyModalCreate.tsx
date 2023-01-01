import React, { ChangeEvent, useState } from 'react';
import { Button, Form, Modal } from 'semantic-ui-react';
import { Duty } from '../../../app/models/duty';
import { v4 as uuid } from 'uuid';

interface Props {
    setDuties: React.Dispatch<React.SetStateAction<Duty[]>>;
    createMode: boolean;
    closeCreateMode: () => void;
}

export default function DutyModalCreate({ setDuties, createMode, closeCreateMode }: Props) {
    const initialState: Duty = {
        id: '',
        position: -1,
        title: '',
        description: '',
        isCompleted: false,
        dateCreation: (new Date()).toString(),
        dateCompletion: '',
        backgroundColor: '#ffffff',
        fontColor: '#000000'
    };

    const [duty, setDuty] = useState<Duty>(initialState);

    function handleSubmit() {
        setDuties((duties) => (
            [...duties, {
                ...duty,
                id: uuid(),
                dateCreation: (new Date()).toString()
            }]
        ));

        closeCreateMode();
    }

    function handleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        const { name, value } = event.target;

        setDuty({ ...duty, [name]: value });
    }

    return (
        <Modal
            centered={false}
            open={createMode}
            onClose={closeCreateMode}
            onOpen={() => setDuty(initialState)}
        >
            <Modal.Header>Create task</Modal.Header>
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
                    <Form.Field>
                        <Form.Input
                            className='input-color'
                            label='Background color'
                            type='color'
                            placeholder='Background color'
                            value={duty.backgroundColor}
                            onChange={handleInputChange}
                            name='backgroundColor'
                        />
                    </Form.Field>
                    <Form.Field>
                        <Form.Input
                            className='input-color'
                            label='Font color'
                            type='color'
                            placeholder='Font color'
                            value={duty.fontColor}
                            onChange={handleInputChange}
                            name='fontColor'
                        />
                    </Form.Field>
                </Form>
            </Modal.Content>
            <Modal.Actions>
                <Button negative onClick={closeCreateMode}>
                    Cancel
                </Button>
                <Button positive onClick={handleSubmit}>
                    Create
                </Button>
            </Modal.Actions>
        </Modal>
    );
}