import { useState } from 'react';
import { Form, Icon } from 'semantic-ui-react';

import TranslateInput, { Language } from './TranslateInput';
import translateKeyboard, { KeyboardLanguage } from '../../app/utils/translateKeyboard';

const arrLanguages: Language[] = [
    { name: 'Ukraine', countryCode: 'ua', keyboardLanguage: KeyboardLanguage.Ukraine },
    { name: 'English', countryCode: 'us', keyboardLanguage: KeyboardLanguage.English },
    { name: 'Russia', countryCode: 'ru', keyboardLanguage: KeyboardLanguage.Russia }
];

export default function TranslateKeyboardPage() {
    const [text, setText] = useState({
        input: '',
        output: ''
    });

    const [selectedLanguages, setSelectedLanguages] = useState({
        input: arrLanguages[1].keyboardLanguage,
        output: arrLanguages[0].keyboardLanguage
    });

    function handleInputText(text: string) {
        setText({
            input: text,
            output: translateKeyboard(text, selectedLanguages.input, selectedLanguages.output)
        });
    }

    function handleClickMenuItem(inputType: 'input' | 'output', keyboardLanguage: KeyboardLanguage) {
        let selectedLang = {
            ...selectedLanguages,
            [inputType]: keyboardLanguage
        };

        if (inputType === 'input' && selectedLanguages.output === keyboardLanguage) {
            selectedLang = {
                input: keyboardLanguage,
                output: selectedLanguages.input
            };
        } else if (inputType === 'output' && selectedLanguages.input === keyboardLanguage) {
            selectedLang = {
                input: selectedLanguages.output,
                output: keyboardLanguage
            };
        }

        setSelectedLanguages(selectedLang);

        setText((state) => ({
            input: state.input,
            output: translateKeyboard(state.input, selectedLang.input, selectedLang.output)
        }));
    }

    function handleClickSwapLanguage() {
        setSelectedLanguages((state) => ({
            input: state.output,
            output: state.input
        }));

        setText((state) => ({
            input: state.output,
            output: translateKeyboard(state.input, selectedLanguages.output, selectedLanguages.input)
        }));
    }

    return (
        <Form className="translate-keyboard">
            <div className="flex-fill">
                <TranslateInput
                    arrLanguages={arrLanguages}
                    selectedLanguage={selectedLanguages.input}
                    menuClassName="mb-2"
                    placeholder="Some text"
                    value={text.input}
                    name="input"
                    onChange={handleInputText}
                    onClickMenuItem={handleClickMenuItem}
                />
            </div>
            <div
                className="translate-keyboard__icon"
                onClick={handleClickSwapLanguage}
            >
                <Icon
                    name="exchange"
                    size="large"
                />
            </div>
            <div className="flex-fill">
                <TranslateInput
                    arrLanguages={arrLanguages}
                    selectedLanguage={selectedLanguages.output}
                    menuClassName="mb-2"
                    placeholder="Translate"
                    value={text.output}
                    name="output"
                    readOnly
                    onClickMenuItem={handleClickMenuItem}
                />
            </div>
        </Form >
    );
}