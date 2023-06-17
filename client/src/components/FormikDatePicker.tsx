import { CSSProperties } from 'react';
import { useField } from 'formik';
import { Form, Label } from 'semantic-ui-react';
import DatePicker, { ReactDatePickerProps } from 'react-datepicker';

interface Props extends Partial<ReactDatePickerProps> {
    name: string;
    label?: string;
    labelStyle?: CSSProperties;
}

export default function FormikDatePicker(
    {
        name,
        label,
        labelStyle,
        ...props
    }: Props
) {
    const [field, meta, helpers] = useField(name);

    return (
        <Form.Field error={meta.touched && !!meta.error}>
            {label && <label style={labelStyle}>{label}</label>}
            <DatePicker
                {...props}
                {...field}
                selected={field.value ? new Date(field.value) : null}
                onChange={(value) => helpers.setValue(value)}
            />
            {meta.error && <Label basic pointing color="red" content={meta.error} />}
        </Form.Field>
    );
}