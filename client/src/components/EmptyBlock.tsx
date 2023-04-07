import { Segment } from 'semantic-ui-react';

interface Props {
    text?: string;
}

export default function EmptyBlock({ text = 'The block is empty' }: Props) {
    return (
        <Segment textAlign="center" inverted secondary>
            {text}
        </Segment>
    );
}