import { useEffect } from 'react';
import { Navigate } from 'react-router-dom';
import { Container, Header, Segment } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';

import { useStore } from '../../app/stores/store';

export default observer(function ServerErrorPage() {
    const { commonStore } = useStore();
    const { error, resetServerError } = commonStore;

    useEffect(() => {
        return () => {
            resetServerError();
        };
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    if (!error) {
        return <Navigate to="/not-found" replace />;
    }

    return (
        <Container>
            <Header as="h1" content="Server Error" />
            <Header
                as="h5"
                sub
                color="red"
                content={error.message}
            />
            {error.details &&
                <Segment>
                    <Header as="h4" content="Stack trace" color="teal" />
                    <code className="mt-1">{error.details}</code>
                </Segment>
            }
        </Container>
    );
});