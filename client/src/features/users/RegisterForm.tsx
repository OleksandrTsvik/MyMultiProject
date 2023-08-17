import { observer } from 'mobx-react-lite';
import { Button, Header } from 'semantic-ui-react';
import { ErrorMessage, Form, Formik, FormikHelpers } from 'formik';
import * as Yup from 'yup';

import { UserRegister } from '../../app/models/user';
import { useStore } from '../../app/stores/store';
import FormikInput from '../../app/common/form/FormikInput';
import ValidationErrors from '../errors/ValidationErrors';

interface Credentials extends UserRegister {
  error: string | null;
}

export default observer(function RegisterForm() {
  const { userStore } = useStore();
  const { register } = userStore;

  const initialValues: Credentials = {
    userName: '',
    email: '',
    password: '',
    error: null
  };

  const validationSchema = Yup.object({
    userName: Yup.string().required('The username is required')
      .matches(/^\S{2,20}$/, 'Username must contain between 2 and 20 characters and no spaces'),
    email: Yup.string().email().required('The email is required'),
    password: Yup.string().required('The password is required')
      .min(6, 'The password must contain at least 6 characters')
  });

  function handleSubmitForm(
    values: Credentials,
    { setErrors, setSubmitting }: FormikHelpers<Credentials>
  ): void | Promise<any> {
    register(values)
      .catch((error) => setErrors({ error }))
      .finally(() => setSubmitting(false));
  }

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmitForm}
    >
      {({ handleSubmit, isSubmitting, isValid, dirty, errors }) => (
        <Form
          className="ui form error"
          onSubmit={handleSubmit}
          autoComplete="off"
        >
          <Header
            as="h2"
            content="Register form"
            textAlign="center"
            color="teal"
          />
          <ErrorMessage
            name="error"
            render={() => <ValidationErrors errors={errors.error} className="mb-3" />}
          />
          <FormikInput
            name="userName"
            label="Username"
          />
          <FormikInput
            name="email"
            label="Email"
          />
          <FormikInput
            name="password"
            label="Password"
            type="password"
          />
          <Button
            type="submit"
            positive
            fluid
            loading={isSubmitting}
            disabled={isSubmitting || !isValid || !dirty}
          >
            Register
          </Button>
        </Form>
      )}
    </Formik>
  );
});