export enum KeyboardLanguage {
    Ukraine,
    English,
    Russia
}

export default function translateKeyboard(
    text: string,
    inputLanguage: KeyboardLanguage,
    outputLanguage: KeyboardLanguage
) {
    if (!text || inputLanguage === outputLanguage) {
        return text;
    }

    let translateText: string[] = [];

    for (let i = 0; i < text.length; i++) {
        let translateLetter = letters.find((letter) => text[i].toLowerCase() === letter[inputLanguage]);

        if (translateLetter) {
            if (text[i] === text[i].toUpperCase()) {
                translateText.push(translateLetter[outputLanguage].toUpperCase());
            } else {
                translateText.push(translateLetter[outputLanguage]);
            }
        } else {
            translateText.push(text[i]);
        }
    }

    return translateText.join('');
}

type TypeLetter = Record<KeyboardLanguage, string>;

const letters: TypeLetter[] = [
    {
        [KeyboardLanguage.Ukraine]: 'й',
        [KeyboardLanguage.English]: 'q',
        [KeyboardLanguage.Russia]: 'й'
    },
    {
        [KeyboardLanguage.Ukraine]: 'ц',
        [KeyboardLanguage.English]: 'w',
        [KeyboardLanguage.Russia]: 'ц'
    },
    {
        [KeyboardLanguage.Ukraine]: 'у',
        [KeyboardLanguage.English]: 'e',
        [KeyboardLanguage.Russia]: 'у'
    },
    {
        [KeyboardLanguage.Ukraine]: 'к',
        [KeyboardLanguage.English]: 'r',
        [KeyboardLanguage.Russia]: 'к'
    },
    {
        [KeyboardLanguage.Ukraine]: 'е',
        [KeyboardLanguage.English]: 't',
        [KeyboardLanguage.Russia]: 'е'
    },
    {
        [KeyboardLanguage.Ukraine]: 'н',
        [KeyboardLanguage.English]: 'y',
        [KeyboardLanguage.Russia]: 'н'
    },
    {
        [KeyboardLanguage.Ukraine]: 'г',
        [KeyboardLanguage.English]: 'u',
        [KeyboardLanguage.Russia]: 'г'
    },
    {
        [KeyboardLanguage.Ukraine]: 'ш',
        [KeyboardLanguage.English]: 'i',
        [KeyboardLanguage.Russia]: 'ш'
    },
    {
        [KeyboardLanguage.Ukraine]: 'щ',
        [KeyboardLanguage.English]: 'o',
        [KeyboardLanguage.Russia]: 'щ'
    },
    {
        [KeyboardLanguage.Ukraine]: 'з',
        [KeyboardLanguage.English]: 'p',
        [KeyboardLanguage.Russia]: 'з'
    },
    {
        [KeyboardLanguage.Ukraine]: 'х',
        [KeyboardLanguage.English]: '[',
        [KeyboardLanguage.Russia]: 'х'
    },
    {
        [KeyboardLanguage.Ukraine]: 'ї',
        [KeyboardLanguage.English]: ']',
        [KeyboardLanguage.Russia]: 'ъ'
    },
    {
        [KeyboardLanguage.Ukraine]: 'ф',
        [KeyboardLanguage.English]: 'a',
        [KeyboardLanguage.Russia]: 'ф'
    },
    {
        [KeyboardLanguage.Ukraine]: 'і',
        [KeyboardLanguage.English]: 's',
        [KeyboardLanguage.Russia]: 'ы'
    },
    {
        [KeyboardLanguage.Ukraine]: 'в',
        [KeyboardLanguage.English]: 'd',
        [KeyboardLanguage.Russia]: 'в'
    },
    {
        [KeyboardLanguage.Ukraine]: 'а',
        [KeyboardLanguage.English]: 'f',
        [KeyboardLanguage.Russia]: 'а'
    },
    {
        [KeyboardLanguage.Ukraine]: 'п',
        [KeyboardLanguage.English]: 'g',
        [KeyboardLanguage.Russia]: 'п'
    },
    {
        [KeyboardLanguage.Ukraine]: 'р',
        [KeyboardLanguage.English]: 'h',
        [KeyboardLanguage.Russia]: 'р'
    },
    {
        [KeyboardLanguage.Ukraine]: 'о',
        [KeyboardLanguage.English]: 'j',
        [KeyboardLanguage.Russia]: 'о'
    },
    {
        [KeyboardLanguage.Ukraine]: 'л',
        [KeyboardLanguage.English]: 'k',
        [KeyboardLanguage.Russia]: 'л'
    },
    {
        [KeyboardLanguage.Ukraine]: 'д',
        [KeyboardLanguage.English]: 'l',
        [KeyboardLanguage.Russia]: 'д'
    },
    {
        [KeyboardLanguage.Ukraine]: 'ж',
        [KeyboardLanguage.English]: ';',
        [KeyboardLanguage.Russia]: 'ж'
    },
    {
        [KeyboardLanguage.Ukraine]: 'є',
        [KeyboardLanguage.English]: '\'',
        [KeyboardLanguage.Russia]: 'э'
    },
    {
        [KeyboardLanguage.Ukraine]: 'я',
        [KeyboardLanguage.English]: 'z',
        [KeyboardLanguage.Russia]: 'я'
    },
    {
        [KeyboardLanguage.Ukraine]: 'ч',
        [KeyboardLanguage.English]: 'x',
        [KeyboardLanguage.Russia]: 'ч'
    },
    {
        [KeyboardLanguage.Ukraine]: 'с',
        [KeyboardLanguage.English]: 'c',
        [KeyboardLanguage.Russia]: 'с'
    },
    {
        [KeyboardLanguage.Ukraine]: 'м',
        [KeyboardLanguage.English]: 'v',
        [KeyboardLanguage.Russia]: 'м'
    },
    {
        [KeyboardLanguage.Ukraine]: 'и',
        [KeyboardLanguage.English]: 'b',
        [KeyboardLanguage.Russia]: 'и'
    },
    {
        [KeyboardLanguage.Ukraine]: 'т',
        [KeyboardLanguage.English]: 'n',
        [KeyboardLanguage.Russia]: 'т'
    },
    {
        [KeyboardLanguage.Ukraine]: 'ь',
        [KeyboardLanguage.English]: 'm',
        [KeyboardLanguage.Russia]: 'ь'
    },
    {
        [KeyboardLanguage.Ukraine]: 'б',
        [KeyboardLanguage.English]: ',',
        [KeyboardLanguage.Russia]: 'б'
    },
    {
        [KeyboardLanguage.Ukraine]: 'ю',
        [KeyboardLanguage.English]: '.',
        [KeyboardLanguage.Russia]: 'ю'
    }
];