import { CSSProperties } from 'react';
import { useField } from 'formik';
import { Form, Label } from 'semantic-ui-react';

import AutoComplete, { AutoCompleteProps } from '../../../components/AutoComplete';

interface Props extends AutoCompleteProps {
  name: string;
  label?: string;
  labelStyle?: CSSProperties;
}

export default function FormikAutoComplete(
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
      <AutoComplete
        {...props}
        {...field}
        value={field.value || null}
        onChange={(event, data) => helpers.setValue(data.value)}
        onBlur={() => helpers.setTouched(true)}
        error={meta.touched && !!meta.error}
      />
      {meta.touched && meta.error && <Label basic pointing color="red" content={meta.error} />}
    </Form.Field>
  );
}