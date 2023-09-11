import { useMemo } from 'react';
import { Label, LabelProps, SemanticCOLORS } from 'semantic-ui-react';

import { defaultStatusColor, statuses } from './StatusDropdown';

export const defaultCounterColor: SemanticCOLORS = 'violet';

interface Props extends LabelProps {
  status?: string | null;
  counter?: boolean;
}

export default function StatusLabel({ status, counter, ...props }: Props) {
  const color: SemanticCOLORS = useMemo(() => {
    if (counter) {
      return defaultCounterColor;
    }

    if (status && statuses.hasOwnProperty(status)) {
      return statuses[status];
    }

    return defaultStatusColor;
  }, [counter, status]);

  if (!status && !counter) {
    return null;
  }

  return (
    <Label
      horizontal
      color={color}
      content={status}
      {...props}
    />
  );
}