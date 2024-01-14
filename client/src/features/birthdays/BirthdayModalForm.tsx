import { useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Modal } from 'semantic-ui-react';
import { Form, Formik, FormikHelpers } from 'formik';
import * as Yup from 'yup';

import { useStore } from '../../app/stores/store';
import ValidationErrors from '../errors/ValidationErrors';
import FormikInput from '../../app/common/form/FormikInput';
import FormikDatePicker from '../../app/common/form/FormikDatePicker';
import FormikTextArea from '../../app/common/form/FormikTextArea';

export interface BirthdayForm {
  fullName: string;
  date: Date;
  note?: string | null;
}

const validationSchema: Yup.Schema<BirthdayForm> = Yup.object({
  fullName: Yup.string().required('The fullName is required'),
  date: Yup.date().required('The date is required'),
  note: Yup.string().optional().nullable(),
});

interface Props {
  title: string;
  textForSubmitBtn: string;
  initialValues: BirthdayForm;
  onSubmit: (
    values: BirthdayForm,
    formikHelpers: FormikHelpers<BirthdayForm>,
  ) => Promise<void>;
}

export default observer(function BirthdayModalForm({
  title,
  textForSubmitBtn,
  initialValues,
  onSubmit,
}: Props) {
  const { modalStore } = useStore();
  const { closeModal } = modalStore;

  const [error, setError] = useState<any>();

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={(values, actions) => {
        onSubmit(values, actions)
          .then(() => closeModal())
          .catch((error) => setError(error))
          .finally(() => actions.setSubmitting(false));
      }}
    >
      {({ handleSubmit, submitForm, isSubmitting, isValid, dirty }) => (
        <>
          <Modal.Header>{title}</Modal.Header>
          <Modal.Content>
            <Form className="ui form" onSubmit={handleSubmit}>
              <ValidationErrors errors={error} className="mb-3" />
              <FormikInput name="fullName" label="FullName" />
              <FormikDatePicker name="date" label="Date" />
              <FormikTextArea name="note" label="Note" />
            </Form>
          </Modal.Content>
          <Modal.Actions>
            <Button
              content="Cancel"
              disabled={isSubmitting}
              onClick={closeModal}
            />
            <Button
              primary
              loading={isSubmitting}
              disabled={isSubmitting || !isValid || !dirty}
              onClick={submitForm}
            >
              {textForSubmitBtn}
            </Button>
          </Modal.Actions>
        </>
      )}
    </Formik>
  );
});
