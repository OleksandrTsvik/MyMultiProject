import React from 'react';
import { Icon, Label, SemanticCOLORS, SemanticICONS } from 'semantic-ui-react';

interface Props {
    name?: SemanticICONS | undefined;
    color?: SemanticCOLORS | undefined;
    value: number;
}

export default function IconPill({ name, color, value }: Props) {
    return (
        <Icon className='position-relative' name={name}>
            {value > 0 &&
                <Label
                    color={color}
                    floating
                    circular
                    style={{ top: '-0.8em', left: '130%' }}
                >
                    {value > 99
                        ? '99+'
                        : value
                    }
                </Label>
            }
        </Icon>
    );
}