import { useState } from 'react';

import { KeyboardLanguage } from '../../app/utils/translateKeyboard';

interface Params {
  initText: {
    input: string;
    output: string;
  };
  initLanguages: {
    input: KeyboardLanguage;
    output: KeyboardLanguage;
  };
  translate: (
    text: string,
    inputLanguage: KeyboardLanguage,
    outputLanguage: KeyboardLanguage
  ) => string;
}

export default function useTranslator(
  {
    initText = { input: '', output: '' },
    initLanguages,
    translate
  }: Params
) {
  const [text, setText] = useState(initText);
  const [selectedLanguages, setSelectedLanguages] = useState(initLanguages);

  function handleInputText(text: string) {
    setText({
      input: text,
      output: translate(text, selectedLanguages.input, selectedLanguages.output)
    });
  }

  function handleChangeLanguage(inputType: 'input' | 'output', keyboardLanguage: KeyboardLanguage) {
    if (inputType === 'input' && selectedLanguages.output === keyboardLanguage) {
      setSelectedLanguages((state) => ({
        input: keyboardLanguage,
        output: state.input
      }));
    } else if (inputType === 'output' && selectedLanguages.input === keyboardLanguage) {
      setSelectedLanguages((state) => ({
        input: state.output,
        output: keyboardLanguage
      }));
    } else {
      setSelectedLanguages((state) => ({
        ...state,
        [inputType]: keyboardLanguage
      }));
    }
  }

  function handleSwapLanguage() {
    setText((state) => ({
      input: state.output,
      output: translate(state.input, selectedLanguages.output, selectedLanguages.input)
    }));

    setSelectedLanguages((state) => ({
      input: state.output,
      output: state.input
    }));
  }

  return {
    text, setText,
    selectedLanguages, setSelectedLanguages,
    handleInputText,
    handleChangeLanguage,
    handleSwapLanguage
  };
}
