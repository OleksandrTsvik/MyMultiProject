import { Link, useParams } from 'react-router-dom';
import { Button, Container, Form, Icon, Input, TextArea } from 'semantic-ui-react';

export default function CategoryItemForm() {
    const { categoryId, itemId } = useParams();

    return (
        <Container>
            <div className="mb-3">
                <Button
                    secondary
                    as={Link} to={`/dictionary/categories/${categoryId}`}
                >
                    <Icon name="angle left" />
                    Back
                </Button>
            </div>
            <Form>
                <Form.Field
                    control={Input}
                    label='Status'
                />
                <Form.Field
                    control={TextArea}
                    label='Text'
                />
                <Form.Field
                    control={TextArea}
                    label='Translation'
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