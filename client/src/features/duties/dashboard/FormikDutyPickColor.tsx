import { ChangeEvent, Dispatch } from 'react';
import { Form } from 'semantic-ui-react';
import { useField } from 'formik';

import { Style } from '../modals/DutyModalCreate';
import PopularColors from '../../../components/PopularColors';

interface Props {
    style: Style;
    setStyle: Dispatch<React.SetStateAction<Style>>;
    onChangeColor?: () => void;
}

export default function FormikDutyPickColor({ style, setStyle, onChangeColor }: Props) {
    const [fieldBackgroundColor, , helpersBackgroundColor] = useField('backgroundColor');
    const [fieldFontColor, , helpersFontColor] = useField('fontColor');

    function handleChangeColor() {
        onChangeColor && onChangeColor();
    }

    function handleInputColor(event: ChangeEvent<HTMLInputElement>) {
        const { name, value } = event.target;
        updateColor(name, value);
    }

    function handleClickColor(property: string, color: string) {
        updateColor(property, color);
        handleChangeColor();
    }

    function updateColor(property: string, value: string) {
        switch (property) {
            case 'fontColor':
                helpersFontColor.setValue(value);
                setStyle({ ...style, 'color': value, 'borderColor': value });
                break;
            default:
                helpersBackgroundColor.setValue(value);
                setStyle({ ...style, [property]: value });
        }
    }

    return (
        <>
            <Form.Field>
                <label style={style}>Background color</label>
                <PopularColors
                    onClickColor={(color) => handleClickColor('backgroundColor', color)}
                />
                <Form.Input
                    {...fieldBackgroundColor}
                    type="color"
                    className="duty__input-color"
                    onInput={handleInputColor}
                    onBlur={handleChangeColor}
                />
            </Form.Field>
            <Form.Field>
                <label style={style}>Font color</label>
                <PopularColors
                    onClickColor={(color) => handleClickColor('fontColor', color)}
                />
                <Form.Input
                    {...fieldFontColor}
                    type="color"
                    className="duty__input-color"
                    onInput={handleInputColor}
                    onBlur={handleChangeColor}
                />
            </Form.Field>
        </>
    );
}