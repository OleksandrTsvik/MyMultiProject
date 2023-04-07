import { Container, Message } from 'semantic-ui-react';

export default function NotFoundPage() {
    return (
        <Container className="flex-fill d-flex">
            <Message
                className="flex-fill d-flex flex-column justify-content-center align-items-center"
                negative
            >
                <Message.Header className="mb-2">
                    404. Page Not Found
                </Message.Header>
                <p>Make sure the address is correct and the page hasn't moved</p>
            </Message>
        </Container>
    );
}