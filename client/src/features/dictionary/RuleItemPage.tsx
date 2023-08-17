import { Link, useParams } from 'react-router-dom';
import { Button, Container, Flag, Header, Label, Segment } from 'semantic-ui-react';

import LinkBack from '../../components/LinkBack';

export default function RuleItemPage() {
    const { ruleId } = useParams();

    return (
        <Container>
            <LinkBack link="/dictionary/rules" />
            <Header as="h2" className="text-center">
                <Label horizontal color="teal">status</Label>
                <Flag name="america" className="mx-2" />
                <div className="d-inline-block">Title --- ID {ruleId}</div>
            </Header>
            <Segment>
                Description
            </Segment>
            <div className="text-end">
                <Button
                    content="Edit"
                    color="blue"
                    as={Link} to={`/dictionary/rules/edit/${ruleId}`}
                />
                <Button
                    content="Delete"
                    color="red"
                />
            </div>
        </Container>
    );
}