import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Container, Header } from 'semantic-ui-react';
import { Form, Formik, FormikHelpers } from 'formik';
import * as Yup from 'yup';

import FormikTextEditor from '../../../app/common/form/FormikTextEditor';
import LinkBack from '../../../components/LinkBack';
import ValidationErrors from '../../errors/ValidationErrors';
import FormikStatusDropdown from '../FormikStatusDropdown';

export interface DictionaryItemForm {
  text: string;
  translation: string;
  status?: string | null;
}

const validationSchema: Yup.Schema<DictionaryItemForm> = Yup.object({
  text: Yup.string().min(9, 'The item text is required').required('The item text is required'),
  translation: Yup.string().min(9, 'The item translation is required').required('The item translation is required'),
  status: Yup.string().optional().nullable()
});

interface Props {
  title: string;
  textForSubmitBtn: string;
  linkToBack: string;
  textToBack?: string;
  initialValues: DictionaryItemForm;
  onSubmit: (values: DictionaryItemForm, formikHelpers: FormikHelpers<DictionaryItemForm>) => void;
}

export default function CategoryItemForm(
  {
    title,
    textForSubmitBtn,
    linkToBack,
    textToBack = 'Back',
    initialValues,
    onSubmit
  }: Props
) {
  const navigate = useNavigate();
  const [error, setError] = useState<any>();

  return (
    <Container>
      <div className="mb-3">
        <LinkBack link={linkToBack} text={textToBack} />
      </div>
      <Header as="h2" className="text-center">{title}</Header>
      <Formik
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={(values, actions) => {
          Promise.resolve(onSubmit(values, actions))
            .then(() => navigate(linkToBack))
            .catch((error) => setError(error))
            .finally(() => actions.setSubmitting(false));
        }}
      >
        {({ handleSubmit, isSubmitting, isValid, dirty }) => (
          <Form className="ui form" onSubmit={handleSubmit}>
            <ValidationErrors errors={error} className="mb-3" />
            <FormikStatusDropdown
              name="status"
              label="Status"
            />
            <FormikTextEditor
              name="text"
              label="Text"
            />
            <FormikTextEditor
              name="translation"
              label="Translation"
            />
            <Button
              type="submit"
              primary
              fluid
              loading={isSubmitting}
              disabled={isSubmitting || !isValid || !dirty}
            >
              {textForSubmitBtn}
            </Button>
          </Form>
        )}
      </Formik>
    </Container>
  );
}