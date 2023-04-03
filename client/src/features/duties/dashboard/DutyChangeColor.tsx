import { ChangeEvent, Dispatch } from "react";
import { Card, Form } from "semantic-ui-react";

import { Duty } from "../../../app/models/duty";
import { useStore } from "../../../app/stores/store";
import { Style } from "../modal/DutyModalCreate";
import PopularColors from "../../../components/PopularColors";

interface Props {
    duty: Duty;
    style: Style;
    setDuty: Dispatch<React.SetStateAction<Duty>>;
    setStyle: Dispatch<React.SetStateAction<Style>>;
}

export default function DutyChangeColor({ duty, setDuty, style, setStyle }: Props) {
    const { dutyStore } = useStore();
    const { changeColor } = dutyStore;

    function handleInputColor(event: ChangeEvent<HTMLInputElement>) {
        const { name, value } = event.target;
        updateColor(name, value);
    }

    function handleClickColor(property: string, color: string) {
        updateColor(property, color);
        changeColor({ ...duty, [property]: color });
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
        <Card.Content
            style={style}
        >
            <Form>
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
                        onBlur={() => changeColor(duty)}
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
                        onBlur={() => changeColor(duty)}
                    />
                </Form.Field>
            </Form>
        </Card.Content>
    );
}