import { FlagNameValues } from 'semantic-ui-react';

export default function getSemanticFlagName(flagName?: string): FlagNameValues {
  let flag: FlagNameValues = 'ukraine';

  if (!flagName) {
    return flag;
  }

  try {
    flag = flagName.toLowerCase() as FlagNameValues;
  } catch (error) {
    console.log(error);
  }

  return flag;
}