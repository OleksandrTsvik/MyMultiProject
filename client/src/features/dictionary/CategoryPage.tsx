import { Link, useParams } from 'react-router-dom';
import { Button, Container, Header, Icon, Label, Table } from 'semantic-ui-react';

import LinkBack from '../../components/LinkBack';

export default function CategoryPage() {
  const { categoryId } = useParams();

  return (
    <Container>
      <div className="mb-3">
        <LinkBack link="/dictionary/categories" />
      </div>
      <Header as="h2" className="text-center">CategoryPage (58) --- ID {categoryId}</Header>
      <div className="text-end mb-3">
        <Button as={Link} to={`/dictionary/categories/${categoryId}/item/add`}>
          <Icon name="add" />
          Add category item
        </Button>
      </div>
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
                      as={Link} to={`/dictionary/categories/${categoryId}/item/edit/${index}`}
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