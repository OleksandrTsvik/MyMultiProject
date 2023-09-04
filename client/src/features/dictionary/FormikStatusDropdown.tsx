import { CSSProperties } from 'react';
import { useField } from 'formik';
import { DropdownProps, Form, Label } from 'semantic-ui-react';

import StatusDropdown from './StatusDropdown';

interface Props extends DropdownProps {
  name: string;
  label?: string;
  labelStyle?: CSSProperties;
  additionalStatuses?: string[];
}

export default function FormikStatusDropdown(
  {
    name,
    label,
    labelStyle,
    additionalStatuses,
    ...props
  }: Props
) {
  const [field, meta, helpers] = useField(name);

  return (
    <Form.Field error={meta.touched && !!meta.error}>
      {label && <label style={labelStyle}>{label}</label>}
      <StatusDropdown
        {...field}
        {...props}
        additionalStatuses={additionalStatuses}
        value={field.value || null}
        onChange={(event, data) => helpers.setValue(data.value)}
        onBlur={() => helpers.setTouched(true)}
        error={meta.touched && !!meta.error}
      />
      {meta.touched && meta.error && <Label basic pointing color="red" content={meta.error} />}
    </Form.Field>
  );
}