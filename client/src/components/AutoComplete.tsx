import { useState } from 'react';
import { Dropdown, DropdownItemProps, DropdownProps } from 'semantic-ui-react';

export type DropdownValue =
  | boolean
  | number
  | string
  | (boolean | number | string)[]
  | undefined;

export interface AutoCompleteProps extends DropdownProps {
  data?: string[];
}

export default function AutoComplete({ data, onChange, ...props }: AutoCompleteProps) {
  const [options, setOptions] = useState<DropdownItemProps[]>(
    data?.map((item) => ({ text: item, value: item })) ||
    props.options || []
  );

  const [currentValue, setCurrentValue] = useState<DropdownValue>(props.defaultValue);

  function handleAddition(event: React.SyntheticEvent<HTMLElement>, data: DropdownProps) {
    const value = data.value?.toString().trim();

    if (!value || options.some((item) => item.text === value)) {
      return;
    }

    setOptions([
      {
        text: value,
        value
      },
      ...options
    ]);
  }

  function handleChange(event: React.SyntheticEvent<HTMLElement>, data: DropdownProps) {
    onChange && onChange(event, data);

    setCurrentValue(data.value);
  }

  return (
    <Dropdown
      options={options}
      search
      selection
      fluid
      allowAdditions
      value={currentValue}
      onAddItem={handleAddition}
      onChange={handleChange}
      {...props}
    />
  );
}