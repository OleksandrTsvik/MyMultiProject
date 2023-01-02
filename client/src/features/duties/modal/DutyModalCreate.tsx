import React, { ChangeEvent, useEffect, useState } from 'react';
import { Button, Form, Modal } from 'semantic-ui-react';
import { Duty } from '../../../app/models/duty';
import { v4 as uuid } from 'uuid';

interface Props {
    setDuties: React.Dispatch<React.SetStateAction<Duty[]>>;
    createMode: boolean;
    closeCreateMode: () => void;
}

interface Style {
    color: string;
    backgroundColor: string;
    borderColor: string;
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

    const initialStyle: Style = {
        color: '#000000',
        backgroundColor: '#ffffff',
        borderColor: '#ffffff'
    };

    const [duty, setDuty] = useState<Duty>(initialState);
    const [style, setStyle] = useState<Style>(initialStyle);

    useEffect(() => {
        setDuty(initialState);
        setStyle(initialStyle);
    }, [createMode]); // eslint-disable-line react-hooks/exhaustive-deps

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

    function handleInputColor(event: ChangeEvent<HTMLInputElement>) {
        const { name, value } = event.target;

        switch (name) {
            case 'fontColor':
                setStyle({ ...style, 'color': value, 'borderColor': value });
                break;
            default:
                setStyle({ ...style, [name]: value });
        }
    }

    return (
        <Modal
            centered={false}
            open={createMode}
            onClose={closeCreateMode}
            style={style}
        >
            <Modal.Header style={style}>Create task</Modal.Header>
            <Modal.Content style={style}>
                <Form>
                    <Form.Field>
                        <label style={style}>Title</label>
                        <Form.Input
                            placeholder='Title'
                            value={duty.title}
                            name='title'
                            onChange={handleInputChange}
                        />
                    </Form.Field>
                    <Form.Field>
                        <label style={style}>Description</label>
                        <Form.TextArea
                            placeholder='Description'
                            value={duty.description}
                            name='description'
                            onChange={handleInputChange}
                            style={{ minHeight: 100 }}
                        />
                    </Form.Field>
                    <Form.Field>
                        <label style={style}>Background color</label>
                        <Form.Input
                            className='input-color'
                            type='color'
                            placeholder='Background color'
                            value={duty.backgroundColor}
                            onChange={handleInputChange}
                            onInput={handleInputColor}
                            name='backgroundColor'
                        />
                    </Form.Field>
                    <Form.Field>
                        <label style={style}>Font color</label>
                        <Form.Input
                            className='input-color'
                            type='color'
                            placeholder='Font color'
                            value={duty.fontColor}
                            onChange={handleInputChange}
                            onInput={handleInputColor}
                            name='fontColor'
                        />
                    </Form.Field>
                </Form>
            </Modal.Content>
            <Modal.Actions style={style}>
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