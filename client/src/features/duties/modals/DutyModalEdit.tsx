import { useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Modal } from 'semantic-ui-react';
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

import { Duty } from '../../../app/models/duty';
import { useStore } from '../../../app/stores/store';
import FormikInput from '../../../app/common/form/FormikInput';
import FormikTextArea from '../../../app/common/form/FormikTextArea';

export default observer(function DutyModalEdit() {
    const { dutyStore } = useStore();
    const { selectedDuty, editDuty, editMode, closeEditMode } = dutyStore;

    const [duty, setDuty] = useState<Duty | undefined>(selectedDuty);

    const validationSchema = Yup.object({
        title: Yup.string().required('The task title is required'),
        description: Yup.string().required('The task description is required')
    });

    useEffect(() => {
        setDuty(selectedDuty);
    }, [selectedDuty]);

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
            <Formik
                initialValues={duty}
                validationSchema={validationSchema}
                enableReinitialize
                onSubmit={editDuty}
            >
                {({ handleSubmit, submitForm, isSubmitting, isValid, dirty }) => (
                    <>
                        <Modal.Content>
                            <Form className="ui form" onSubmit={handleSubmit}>
                                <FormikInput
                                    name="title"
                                    label="Title"
                                    placeholder="Title"
                                />
                                <FormikTextArea
                                    name="description"
                                    label="Description"
                                    placeholder="Description"
                                    style={{ minHeight: 100 }}
                                />
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
                                loading={isSubmitting}
                                disabled={isSubmitting || !isValid || !dirty}
                                onClick={submitForm}
                            >
                                Update
                            </Button>
                        </Modal.Actions>
                    </>
                )}
            </Formik>
        </Modal>
    );
});