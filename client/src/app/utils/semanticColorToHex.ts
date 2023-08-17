import { SemanticCOLORS } from 'semantic-ui-react';

export default function semanticColorToHex(color: string) {
  const semanticColor = semanticColors.find((item) => item.semantic === color);

  if (semanticColor) {
    return semanticColor.hex;
  }
}

export interface SemanticColor {
  semantic: SemanticCOLORS;
  hex: string;
}

export const semanticColors: SemanticColor[] = [
  { semantic: 'red', hex: '#db2828' },
  { semantic: 'orange', hex: '#f2711c' },
  { semantic: 'yellow', hex: '#fbbd08' },
  { semantic: 'olive', hex: '#b5cc18' },
  { semantic: 'green', hex: '#21ba45' },
  { semantic: 'teal', hex: '#00b5ad' },
  { semantic: 'blue', hex: '#2185d0' },
  { semantic: 'violet', hex: '#6435c9' },
  { semantic: 'purple', hex: '#a333c8' },
  { semantic: 'pink', hex: '#e03997' },
  { semantic: 'brown', hex: '#a5673f' },
  { semantic: 'grey', hex: '#767676' },
  { semantic: 'black', hex: '#1b1c1d' }
];