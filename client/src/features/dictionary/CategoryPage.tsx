import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Link, Navigate, useParams } from 'react-router-dom';
import { Button, Container, Header, Icon, Table } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import ModalConfirm from '../../app/common/modals/ModalConfirm';
import LinkBack from '../../components/LinkBack';
import Loading from '../../components/Loading';
import EmptyBlock from '../../components/EmptyBlock';
import CustomFlag from '../../components/CustomFlag';
import StatusLabel from './StatusLabel';

export default observer(function CategoryPage() {
  const { categoryId } = useParams();

  const { modalStore, dictionaryStore } = useStore();

  const { openModal } = modalStore;
  const {
    loadItems, loadingItems,
    items, itemsSortByPosition,
    resetItems, loadCategoryDetails,
    loadingCategoryDetails, categoryDetails,
    resetCategoryDetails
  } = dictionaryStore;

  useEffect(() => {
    loadItems()
  }, [loadItems]);

  useEffect(() => {
    if (categoryId) {
      loadCategoryDetails(categoryId);
    }
  }, [categoryId, loadCategoryDetails]);

  useEffect(() => {
    return () => {
      resetItems();
      resetCategoryDetails();
    }
  }, [resetItems, resetCategoryDetails]);

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

  if (loadingCategoryDetails || loadingItems) {
    return <Loading content="Loading category item..." />;
  }

  if (!categoryId || !categoryDetails) {
    return <Navigate to="/not-found" replace />;
  }

  return (
    <Container>
      <div className="mb-3">
        <LinkBack link="/dictionary/categories" />
      </div>
      <Header as="h2" className="text-center">
        <CustomFlag className="flag__medium" strName={categoryDetails.language} />
        {categoryDetails.title} ({categoryDetails.countItems})
      </Header>
      <div className="text-end mb-3">
        <Button as={Link} to={`/dictionary/categories/${categoryId}/item/add`}>
          <Icon name="add" />
          Add category item
        </Button>
      </div>
      {items.size === 0
        ? <EmptyBlock />
        : <div className="table-responsive">
          <Table celled striped selectable unstackable>
            <Table.Body>
              {itemsSortByPosition.map((item) => (
                <Table.Row key={item.id} verticalAlign="top">
                  <Table.Cell collapsing>
                    <Icon name="block layout" />
                  </Table.Cell>
                  <Table.Cell>
                    <StatusLabel status={item.status} />
                    {item.text}
                  </Table.Cell>
                  <Table.Cell>
                    {item.translation}
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <div className="d-flex gap-2">
                      <Button
                        className="m-0"
                        icon="pencil alternate"
                        color="blue"
                        as={Link} to={`/dictionary/categories/${categoryId}/item/edit/${item.id}`}
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
      }
    </Container>
  );
});