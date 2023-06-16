import { Container, Header, Segment } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';

import { useStore } from '../../app/stores/store';

export default observer(function ServerErrorPage() {
    const { commonStore } = useStore();
    const { error } = commonStore;

    return (
        <Container>
            <Header as="h1" content="Server Error" />
            <Header
                as="h5"
                sub
                color="red"
                content={error?.message}
            />
            {error?.details &&
                <Segment>
                    <Header as="h4" content="Stack trace" color="teal" />
                    <code className="mt-1">{error.details}</code>
                </Segment>
            }
        </Container>
    );
});