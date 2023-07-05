import { Segment } from 'semantic-ui-react';

interface Props {
    text?: string;
    className?: string;
}

export default function EmptyBlock({ text = 'The block is empty', className }: Props) {
    return (
        <Segment
            inverted
            secondary
            textAlign="center"
            className={'w-100' + (className ? ' ' + className : '')}
        >
            {text}
        </Segment>
    );
}