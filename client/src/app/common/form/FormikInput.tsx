import { CSSProperties } from 'react';
import { useField } from 'formik';
import { Form, FormInputProps } from 'semantic-ui-react';

interface Props extends FormInputProps {
    name: string;
    label?: string;
    labelStyle?: CSSProperties;
}

export default function FormikInput(
    {
        name,
        label,
        labelStyle,
        ...props
    }: Props
) {
    const [field, meta] = useField(name);

    return (
        <Form.Field error={meta.touched && !!meta.error}>
            {label && <label style={labelStyle}>{label}</label>}
            <Form.Input
                {...props}
                {...field}
                error={meta.touched ? meta.error : null}
            />
        </Form.Field>
    );
}