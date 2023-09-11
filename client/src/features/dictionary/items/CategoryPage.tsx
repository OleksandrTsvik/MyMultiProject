import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Link, Navigate, useParams } from 'react-router-dom';
import { Button, Container, Header, Icon } from 'semantic-ui-react';

import { useStore } from '../../../app/stores/store';
import LinkBack from '../../../components/LinkBack';
import Loading from '../../../components/Loading';
import EmptyBlock from '../../../components/EmptyBlock';
import CustomFlag from '../../../components/CustomFlag';
import ItemsList from './ItemsList';

export default observer(function CategoryPage() {
  const { categoryId } = useParams();

  const { dictionaryStore } = useStore();
  const {
    loadItems, loadingItems,
    items, resetItems,
    loadCategoryDetails, loadingCategoryDetails,
    categoryDetails, resetCategoryDetails
  } = dictionaryStore;

  useEffect(() => {
    if (categoryId) {
      loadItems(categoryId);
    }
  }, [categoryId, loadItems]);

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
        : <ItemsList />
      }
    </Container>
  );
});