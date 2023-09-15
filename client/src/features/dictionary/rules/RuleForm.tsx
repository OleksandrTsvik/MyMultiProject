import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Container, Header } from 'semantic-ui-react';
import { Form, Formik, FormikHelpers } from 'formik';
import * as Yup from 'yup';

import FormikCountryDropdown from '../../../app/common/form/FormikCountryDropdown';
import FormikInput from '../../../app/common/form/FormikInput';
import FormikTextEditor from '../../../app/common/form/FormikTextEditor';
import FormikStatusDropdown from '../FormikStatusDropdown';
import LinkBack from '../../../components/LinkBack';
import ValidationErrors from '../../errors/ValidationErrors';

export interface GrammarRuleForm {
  title: string;
  description: string;
  language: string;
  status?: string;
}

const validationSchema: Yup.Schema<GrammarRuleForm> = Yup.object({
  title: Yup.string().required('The rule title is required'),
  description: Yup.string()
    .min(9, 'The rule description is required')
    .required('The rule description is required'),
  language: Yup.string().required('The rule language is required')
});

interface Props {
  title: string;
  textForSubmitBtn: string;
  linkToBack: string;
  textToBack?: string;
  initialValues: GrammarRuleForm;
  onSubmit: (values: GrammarRuleForm, formikHelpers: FormikHelpers<GrammarRuleForm>) => Promise<void>;
}

export default function RuleForm(
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
          onSubmit(values, actions)
            .then(() => navigate(linkToBack))
            .catch((error) => setError(error))
            .finally(() => actions.setSubmitting(false));
        }}
      >
        {({ handleSubmit, isSubmitting, isValid, dirty }) => (
          <Form className="ui form" onSubmit={handleSubmit}>
            <ValidationErrors errors={error} className="mb-3" />
            <FormikCountryDropdown
              name="language"
              label="Language"
            />
            <FormikStatusDropdown
              name="status"
              label="Status"
            />
            <FormikInput
              name="title"
              label="Title"
            />
            <FormikTextEditor
              name="description"
              label="Description"
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