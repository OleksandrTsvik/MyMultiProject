import { Link, useParams } from 'react-router-dom';
import { Button, Container, Header, Icon, Label, Table } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import ModalConfirm from '../../app/common/modals/ModalConfirm';
import LinkBack from '../../components/LinkBack';

export default function CategoryPage() {
  const { categoryId } = useParams();

  const { modalStore } = useStore();
  const { openModal } = modalStore;

  function handleOpenDeleteCategoryItem() {
    openModal(
      <ModalConfirm
        content={`Delete the category item ${'title'}.`}
        onConfirm={() => console.log('onConfirm Category Item')}
      />,
      {},
      true
    );
  }

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
                      className="m-0"
                      icon="pencil alternate"
                      color="blue"
                      as={Link} to={`/dictionary/categories/${categoryId}/item/edit/${index}`}
                    />
                    <Button
                      className="m-0"
                      icon="trash alternate"
                      color="red"
                      onClick={handleOpenDeleteCategoryItem}
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