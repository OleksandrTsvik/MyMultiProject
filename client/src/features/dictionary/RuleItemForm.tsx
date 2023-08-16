import { Link, useParams } from 'react-router-dom';
import { Button, Container, Form, Icon, Input, TextArea } from 'semantic-ui-react';

export default function RuleItemForm() {
    const { ruleId } = useParams();

    return (
        <Container>
            <div className="mb-3">
                <Button
                    secondary
                    as={Link} to="/dictionary/rules/"
                >
                    <Icon name="angle left" />
                    Back
                </Button>
            </div>
            <Form>
                <Form.Field
                    control={Input}
                    label='Language'
                />
                <Form.Field
                    control={Input}
                    label='Status'
                />
                <Form.Field
                    control={Input}
                    label='Title'
                />
                <Form.Field
                    control={TextArea}
                    label='Description'
                />
                <Button
                    type="submit"
                    primary
                    fluid
                >
                    Add | Edit
                </Button>
            </Form>
        </Container>
    );
}