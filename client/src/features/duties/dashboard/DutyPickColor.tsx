import { ChangeEvent, Dispatch } from "react";
import { Form } from "semantic-ui-react";

import { Duty } from "../../../app/models/duty";
import { Style } from "../modal/DutyModalCreate";
import PopularColors from "../../../components/PopularColors";

interface Props {
    duty: Duty;
    style: Style;
    setDuty: Dispatch<React.SetStateAction<Duty>>;
    setStyle: Dispatch<React.SetStateAction<Style>>;
    onChangeColor?: (duty: Duty) => void;
}

export default function DutyPickColor({ duty, setDuty, style, setStyle, onChangeColor }: Props) {
    function handleChangeColor(duty: Duty) {
        onChangeColor && onChangeColor(duty);
    }

    function handleInputColor(event: ChangeEvent<HTMLInputElement>) {
        const { name, value } = event.target;
        updateColor(name, value);
    }

    function handleClickColor(property: string, color: string) {
        updateColor(property, color);
        handleChangeColor({ ...duty, [property]: color });
    }

    function updateColor(property: string, value: string) {
        setDuty({ ...duty, [property]: value });

        switch (property) {
            case 'fontColor':
                setStyle({ ...style, 'color': value, 'borderColor': value });
                break;
            default:
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
                    type="color"
                    className="input__color"
                    name="backgroundColor"
                    value={duty.backgroundColor}
                    onInput={handleInputColor}
                    onBlur={() => handleChangeColor(duty)}
                />
            </Form.Field>
            <Form.Field>
                <label style={style}>Font color</label>
                <PopularColors
                    onClickColor={(color) => handleClickColor('fontColor', color)}
                />
                <Form.Input
                    type="color"
                    className="input__color"
                    name="fontColor"
                    value={duty.fontColor}
                    onInput={handleInputColor}
                    onBlur={() => handleChangeColor(duty)}
                />
            </Form.Field>
        </>
    );
}