import { CSSProperties, SyntheticEvent, useState } from 'react';
import { Dropdown, DropdownItemProps, DropdownProps, SemanticCOLORS } from 'semantic-ui-react';

import semanticColorToHex from '../../app/utils/semanticColorToHex';
import { DropdownValue } from '../../components/AutoComplete';

export const defaultStatusColor: SemanticCOLORS = 'teal';

interface Props extends DropdownProps {
  additionalStatuses?: string[];
}

export default function StatusDropdown(
  {
    additionalStatuses = [],
    onChange,
    ...props
  }: Props
) {
  const statusesKeys = Object.keys(statuses);
  const [currentValue, setCurrentValue] = useState<DropdownValue>(props.value);

  const [options, setOptions] = useState<DropdownItemProps[]>([
    ...additionalStatuses
      .filter((status) => !statusesKeys.includes(status))
      .map((status) => getDropdownItem(status, defaultStatusColor)),
    ...statusesKeys.map((status) => getDropdownItem(status, statuses[status]))
  ]);

  function getDropdownItem(status: string, color: string): DropdownItemProps {
    return {
      key: status,
      label: { color, empty: true, circular: true },
      text: status,
      value: status,
      onClick: (event, data) => setCurrentValue(data.value)
    };
  }

  function handleAddition(event: SyntheticEvent, data: DropdownProps) {
    const value = data.value?.toString().trim();

    if (!value || options.some((item) => item.text === value)) {
      return;
    }

    setCurrentValue(value);

    setOptions([
      getDropdownItem(value, defaultStatusColor),
      ...options
    ]);
  }

  function handleChange(event: SyntheticEvent<HTMLElement>, data: DropdownProps) {
    if (event.cancelable) {
      onChange && onChange(event, data);
    }

    setCurrentValue((state) => {
      // if nothing was selected, then you do not need to select the first item
      if (!state) {
        return '';
      }

      return data.value;
    });
  }

  return (
    <Dropdown
      fluid
      search
      selection
      clearable
      allowAdditions
      placeholder="Select status or add your own"
      {...props}
      options={options}
      value={currentValue}
      className={currentValue ? 'status-dropdown__placeholder' : ''}
      style={getStatusCSS(currentValue?.toString())}
      onChange={handleChange}
      onAddItem={handleAddition}
    />
  );
}

export function getStatusCSS(status?: string): CSSProperties | undefined {
  if (!status) {
    return;
  }

  if (statuses.hasOwnProperty(status)) {
    return {
      color: '#ffffff',
      backgroundColor: semanticColorToHex(statuses[status])
    };
  }

  return {
    color: '#ffffff',
    backgroundColor: semanticColorToHex(defaultStatusColor)
  };
}

export interface Status {
  [key: string]: SemanticCOLORS;
}

export const statuses: Status = {
  Remember: 'green',
  Repeat: 'yellow',
  Study: 'blue',
  New: 'orange',
  Forgot: 'red'
};