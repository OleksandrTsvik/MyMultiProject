import { CSSProperties } from 'react';
import { useField } from 'formik';
import { Form, FormTextAreaProps } from 'semantic-ui-react';

interface Props extends FormTextAreaProps {
  name: string;
  label?: string;
  labelStyle?: CSSProperties;
}

export default function FormikTextArea(
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
      <Form.TextArea
        {...props}
        {...field}
        error={meta.touched ? meta.error : null}
      />
    </Form.Field>
  );
}