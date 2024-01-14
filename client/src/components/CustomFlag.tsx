import { useMemo } from 'react';
import { Flag, FlagNameValues, FlagProps } from 'semantic-ui-react';

import getSemanticFlagName from '../app/utils/getSemanticFlagName';

interface BaseProps extends FlagProps {}

interface NameProps extends BaseProps {
  strName?: never;
}

interface StrProps extends Omit<BaseProps, 'name'> {
  strName: string;
  name?: never;
}

// require one of two properties to exist
type Props = NameProps | StrProps;

export default function CustomFlag({ name, strName, ...props }: Props) {
  const flagName: FlagNameValues | null = useMemo(() => {
    if (name) {
      return name;
    }

    return getSemanticFlagName(strName);
  }, [name, strName]);

  if (!flagName) {
    return null;
  }

  return <Flag {...props} name={flagName} />;
}
