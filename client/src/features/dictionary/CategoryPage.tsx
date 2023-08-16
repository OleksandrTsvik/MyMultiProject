import { Link, useNavigate, useParams } from 'react-router-dom';
import { Button, Container, Header, Icon, Label, Table } from 'semantic-ui-react';

export default function CategoryPage() {
    const { categoryId } = useParams();
    const navigate = useNavigate();

    return (
        <Container>
            <div className="mb-3">
                <Button
                    secondary
                    as={Link} to="/dictionary/categories"
                >
                    <Icon name="angle left" />
                    Back
                </Button>
            </div>
            <Header as="h2" className="text-center">CategoryPage (58) --- ID {categoryId}</Header>
            <div className="table-responsive">
                <Table celled striped selectable unstackable>
                    <Table.Body>
                        {Array(15).fill(null).map((_, index) => (
                            <Table.Row key={index} verticalAlign="top">
                                <Table.Cell collapsing>
                                    <Icon name="block layout" />
                                </Table.Cell>
                                <Table.Cell>
                                    <Label horizontal color="teal">Status</Label>
                                    Text
                                </Table.Cell>
                                <Table.Cell>
                                    Translation
                                </Table.Cell>
                                <Table.Cell collapsing>
                                    <div className="d-flex gap-2">
                                        <Button
                                            icon="pencil alternate"
                                            color="blue"
                                            className="m-0"
                                            onClick={() => navigate(`/dictionary/categories/${categoryId}/item/${index}`)}
                                        />
                                        <Button
                                            icon="trash alternate"
                                            color="red"
                                            className="m-0"
                                        />
                                    </div>
                                </Table.Cell>
                            </Table.Row>
                        ))}
                    </Table.Body>
                </Table>
            </div>
        </Container>
    );
}