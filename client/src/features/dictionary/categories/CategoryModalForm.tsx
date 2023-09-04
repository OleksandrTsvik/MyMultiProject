import { useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Modal } from 'semantic-ui-react';
import { Form, Formik, FormikHelpers } from 'formik';
import * as Yup from 'yup';

import { useStore } from '../../../app/stores/store';
import FormikInput from '../../../app/common/form/FormikInput';
import FormikCountryDropdown from '../../../app/common/form/FormikCountryDropdown';
import ValidationErrors from '../../errors/ValidationErrors';

export interface DictionaryCategoryForm {
  title: string;
  language: string;
}

const validationSchema: Yup.Schema<DictionaryCategoryForm> = Yup.object({
  title: Yup.string().required('The category title is required'),
  language: Yup.string().required('The category language is required')
});

interface Props {
  title: string;
  textForSubmitBtn: string;
  initialValues: DictionaryCategoryForm;
  onSubmit: (values: DictionaryCategoryForm, formikHelpers: FormikHelpers<DictionaryCategoryForm>) => Promise<void>;
}

export default observer(function CategoryModalForm(
  {
    title,
    textForSubmitBtn,
    initialValues,
    onSubmit
  }: Props
) {
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
              <FormikCountryDropdown
                name="language"
                label="Language"
              />
              <FormikInput
                name="title"
                label="Title"
              />
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