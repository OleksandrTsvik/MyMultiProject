import { FlagNameValues } from 'semantic-ui-react';

import { countries } from '../../components/CountryDropdown';

export default function getSemanticFlagName(flagName: string): FlagNameValues | null {
  const flag = flagName.toLowerCase();

  if (isFlagNameValue(flag as FlagNameValues)) {
    return flag as FlagNameValues;
  }

  return null;
}

export function isFlagNameValue(value: FlagNameValues): value is FlagNameValues {
  return validFlagValues.includes(value);
}

export const validFlagValues: FlagNameValues[] = countries.reduce((accumulator, country) => {
  accumulator.push(country.name.toLowerCase() as FlagNameValues);
  accumulator.push(country.countryCode.toLowerCase() as FlagNameValues);

  if (country.alias) {
    accumulator.push(country.alias.toLowerCase() as FlagNameValues);
  }

  return accumulator;
}, [] as FlagNameValues[]);