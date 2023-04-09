import { CSSProperties, ChangeEvent } from 'react';
import { Button, Flag, FlagNameValues, Form, Icon, Menu, Popup, SemanticCOLORS } from 'semantic-ui-react';
import { IconSizeProp } from 'semantic-ui-react/dist/commonjs/elements/Icon/Icon';

import { KeyboardLanguage } from '../../app/utils/translateKeyboard';

export interface Language {
    name: string;
    countryCode: FlagNameValues;
    keyboardLanguage: KeyboardLanguage;
}

interface Props {
    arrLanguages: Language[];
    selectedLanguage: KeyboardLanguage;

    menuPointing?: boolean;
    menuSecondary?: boolean;
    menuColor?: SemanticCOLORS;
    menuClassName?: string;
    onClickMenuItem: (inputType: 'input' | 'output', keyboardLanguage: KeyboardLanguage) => void;

    style?: CSSProperties;
    placeholder: string;
    name: 'input' | 'output';
    readOnly?: boolean;
    value: string;
    onChange?: (text: string) => void;

    buttonSize?: IconSizeProp;
}

export default function TranslateInput(
    {
        arrLanguages,
        selectedLanguage,
        menuPointing = true,
        menuSecondary = true,
        menuColor = 'teal',
        menuClassName,
        onClickMenuItem,
        style = { minHeight: 250 },
        placeholder,
        name,
        readOnly,
        value,
        onChange,
        buttonSize = 'large'
    }: Props
) {
    function handleChange(event: ChangeEvent<HTMLTextAreaElement>) {
        onChange && onChange(event.target.value);
    }

    async function handlePasteText() {
        try {
            onChange && onChange(await navigator.clipboard.readText());
        } catch (error) {
            console.error(error);
        }
    }

    async function handleCopyText() {
        try {
            await navigator.clipboard.writeText(value);
        } catch (error) {
            console.error(error);
        }
    }

    function handleClearText() {
        onChange && onChange('');
    }

    return (
        <>
            <Menu
                pointing={menuPointing}
                secondary={menuSecondary}
                color={menuColor}
                className={menuClassName}
            >
                {arrLanguages.map((language, index) => (
                    <Menu.Item
                        key={index}
                        icon={<Flag name={language.countryCode} />}
                        name={language.name}
                        active={language.keyboardLanguage === selectedLanguage}
                        className="fw-normal"
                        onClick={() => onClickMenuItem(name, language.keyboardLanguage)}
                    />
                ))}
            </Menu>
            <Form.Field>
                <Form.TextArea
                    style={style}
                    placeholder={placeholder}
                    value={value}
                    name={name}
                    readOnly={readOnly}
                    onChange={handleChange}
                />
            </Form.Field>
            <div className="translate-input__buttons">
                {!readOnly &&
                    <Popup
                        content="Paste"
                        trigger={
                            <Button icon inverted color="blue"
                                onClick={handlePasteText}
                            >
                                <Icon name="paste" size={buttonSize} />
                            </Button>
                        }
                    />
                }
                <Popup
                    content="Copy"
                    trigger={
                        <Button icon inverted color="green"
                            onClick={handleCopyText}
                        >
                            <Icon name="copy outline" size={buttonSize} />
                        </Button>
                    }
                />
                {!readOnly &&
                    <Popup
                        content="Clear"
                        trigger={
                            <Button icon color="red"
                                onClick={handleClearText}
                            >
                                <Icon name="remove" size={buttonSize} />
                            </Button>
                        }
                    />
                }
            </div>
        </>
    );
}