import { ChangeEvent, useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Form, Modal } from 'semantic-ui-react';

import { Duty } from '../../../app/models/duty';
import { useStore } from '../../../app/stores/store';
import DutyPickColor from '../dashboard/DutyPickColor';

export interface Style {
    color: string;
    backgroundColor: string;
    borderColor: string;
}

const initialState: Duty = {
    id: '',
    position: -1,
    title: '',
    description: '',
    isCompleted: false,
    dateCreation: (new Date()).toISOString(),
    dateCompletion: null,
    backgroundColor: '#ffffff',
    fontColor: '#000000'
};

export const initialStyle: Style = {
    color: '#000000',
    backgroundColor: '#ffffff',
    borderColor: '#ffffff'
};

export default observer(function DutyModalCreate() {
    const { dutyStore } = useStore();
    const { createLoading, createDuty, createMode, closeCreateMode } = dutyStore;

    const [duty, setDuty] = useState<Duty>(initialState);
    const [style, setStyle] = useState<Style>(initialStyle);

    useEffect(() => {
        setDuty(initialState);
        setStyle(initialStyle);
    }, [createMode]); // eslint-disable-line react-hooks/exhaustive-deps

    function handleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        const { name, value } = event.target;

        setDuty({ ...duty, [name]: value });
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
                            placeholder="Title"
                            value={duty.title}
                            name="title"
                            onChange={handleInputChange}
                        />
                    </Form.Field>
                    <Form.Field>
                        <label style={style}>Description</label>
                        <Form.TextArea
                            style={{ minHeight: 100 }}
                            placeholder="Description"
                            value={duty.description}
                            name="description"
                            onChange={handleInputChange}
                        />
                    </Form.Field>
                    <DutyPickColor
                        duty={duty}
                        style={style}
                        setDuty={setDuty}
                        setStyle={setStyle}
                    />
                </Form>
            </Modal.Content>
            <Modal.Actions style={style}>
                <Button
                    negative
                    onClick={closeCreateMode}
                >
                    Cancel
                </Button>
                <Button
                    positive
                    loading={createLoading}
                    disabled={createLoading}
                    onClick={() => createDuty(duty)}
                >
                    Create
                </Button>
            </Modal.Actions>
        </Modal>
    );
});