import { useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Modal } from 'semantic-ui-react';
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

import { Duty } from '../../../app/models/duty';
import { useStore } from '../../../app/stores/store';
import FormikInput from '../../../app/common/form/FormikInput';
import FormikTextArea from '../../../app/common/form/FormikTextArea';
import FormikDutyPickColor from '../dashboard/FormikDutyPickColor';

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
  dateCreation: new Date(),
  dateCompletion: null,
  backgroundColor: '#ffffff',
  fontColor: '#000000'
};

export const initialStyle: Style = {
  color: '#000000',
  backgroundColor: '#ffffff',
  borderColor: '#000000'
};

export default observer(function DutyModalCreate() {
  const { dutyStore } = useStore();
  const { createLoading, createDuty, createMode, closeCreateMode } = dutyStore;

  const [duty, setDuty] = useState<Duty>(initialState);
  const [style, setStyle] = useState<Style>(initialStyle);

  const validationSchema = Yup.object({
    title: Yup.string().required('The task title is required'),
    description: Yup.string().required('The task description is required')
  });

  useEffect(() => {
    setDuty(initialState);
    setStyle(initialStyle);
  }, [createMode]); // eslint-disable-line react-hooks/exhaustive-deps

  return (
    <Modal
      centered={false}
      open={createMode}
      onClose={closeCreateMode}
      style={style}
    >
      <Modal.Header style={style}>Create task</Modal.Header>
      <Formik
        initialValues={duty}
        validationSchema={validationSchema}
        enableReinitialize
        onSubmit={createDuty}
      >
        {({ handleSubmit, submitForm, isValid, dirty }) => (
          <>
            <Modal.Content style={style}>
              <Form className="ui form" onSubmit={handleSubmit}>
                <FormikInput
                  name="title"
                  label="Title"
                  labelStyle={style}
                  placeholder="Title"
                />
                <FormikTextArea
                  name="description"
                  label="Description"
                  labelStyle={style}
                  placeholder="Description"
                  style={{ minHeight: 100 }}
                />
                <FormikDutyPickColor
                  style={style}
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
                disabled={createLoading || !isValid || !dirty}
                onClick={submitForm}
              >
                Create
              </Button>
            </Modal.Actions>
          </>
        )}
      </Formik>
    </Modal>
  );
});