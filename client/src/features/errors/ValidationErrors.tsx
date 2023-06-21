import { Header, Label, LabelProps, Message, MessageProps } from 'semantic-ui-react';

interface Props extends Omit<MessageProps, 'icon'>, Omit<LabelProps, 'attached'> {
    errors: string | string[] | undefined;
    title?: string;
    titleClassName?: string;
}

export default function ValidationErrors({ errors, title, titleClassName, ...props }: Props) {
    if (!errors) {
        return null;
    }

    if (Array.isArray(errors) && errors.length === 0) {
        return null;
    }

    if (
        (!Array.isArray(errors) && typeof errors !== 'string') ||
        (Array.isArray(errors) && errors.some((error) => typeof error !== 'string'))
    ) {
        errors = "An error occurred, please check the data you entered";
    }

    if (typeof errors === 'string' || (Array.isArray(errors) && errors.length === 1)) {
        return (
            <>
                {title && <Header color="red" content={title} className={titleClassName} />}
                <Label
                    basic
                    size="large"
                    color="red"
                    content={errors}
                    {...props}
                    className={"w-100 mx-0" + (props.className ? ` ${props.className}` : '')}
                />
            </>
        );
    }

    return (
        <Message error {...props}>
            {title && <Message.Header content={title} className={titleClassName} />}
            <Message.List>
                {Array.isArray(errors)
                    ? errors.map((err, i) => (
                        <Message.Item key={i}>{err}</Message.Item>
                    ))
                    : <Message.Item>{errors}</Message.Item>
                }
            </Message.List>
        </Message>
    );
}