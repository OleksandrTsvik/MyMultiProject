import { observer } from 'mobx-react-lite';
import { ErrorMessage, Form, Formik, FormikHelpers } from 'formik';
import { Button, Header } from 'semantic-ui-react';

import { UserLogin } from '../../app/models/user';
import { useStore } from '../../app/stores/store';
import FormikInput from '../../app/common/form/FormikInput';
import ValidationErrors from '../errors/ValidationErrors';

interface Credentials extends UserLogin {
    error: string | null;
}

export default observer(function LoginForm() {
    const { userStore } = useStore();
    const { login } = userStore;

    const initialValues: Credentials = {
        email: 'oleksandr.zwick@gmail.com',
        password: 'Pa$$w0rd',
        error: null
    };

    function handleSubmitForm(
        values: Credentials,
        { setErrors, setSubmitting }: FormikHelpers<Credentials>
    ): void | Promise<any> {
        login(values)
            .catch(() => setErrors({ error: 'Invalid email or password' }))
            .finally(() => setSubmitting(false));
    }

    return (
        <Formik
            initialValues={initialValues}
            onSubmit={handleSubmitForm}
        >
            {({ handleSubmit, isSubmitting, errors }) => (
                <Form
                    className="ui form"
                    onSubmit={handleSubmit}
                    autoComplete="off"
                >
                    <Header
                        as="h2"
                        content="Login form"
                        textAlign="center"
                        color="teal"
                    />
                    <ErrorMessage
                        name="error"
                        render={() => <ValidationErrors errors={errors.error} className="mb-3" />}
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
                    >
                        Login
                    </Button>
                </Form>
            )}
        </Formik>
    );
});