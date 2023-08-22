import { CSSProperties, SyntheticEvent, useState } from 'react';
import { Dropdown, DropdownItemProps, DropdownProps, SemanticCOLORS } from 'semantic-ui-react';

import semanticColorToHex from '../../app/utils/semanticColorToHex';

export type DropdownValue =
  | boolean
  | number
  | string
  | (boolean | number | string)[]
  | undefined;

export const defaultStatusColor: SemanticCOLORS = 'teal';

interface Props {
  additionalStatuses?: string[];
  dropdownProps?: DropdownProps;
}

export default function StatusDropdown(
  {
    additionalStatuses = [],
    dropdownProps
  }: Props
) {
  const [currentValue, setCurrentValue] = useState<DropdownValue>();

  const [options, setOptions] = useState<DropdownItemProps[]>([
    ...additionalStatuses.map((status) => getDropdownItem(status, defaultStatusColor)),
    ...Object.keys(statuses).map((status) => getDropdownItem(status, statuses[status]))
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

    setOptions([
      getDropdownItem(value, defaultStatusColor),
      ...options
    ]);
  }

  function handleChange(event: SyntheticEvent, data: DropdownProps) {
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
      {...dropdownProps}
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