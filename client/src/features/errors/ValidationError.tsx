import { Message } from 'semantic-ui-react';

interface Props {
    errors: string[];
}

export default function ValidationError({ errors }: Props) {
    return (
        <Message error>
            {errors.length > 0 &&
                <>
                    <Message.Header>
                        Validation Error Array
                    </Message.Header>
                    <Message.List>
                        {errors.map((err, i) => (
                            <Message.Item key={i}>{err}</Message.Item>
                        ))}
                    </Message.List>
                </>
            }
        </Message>
    );
}