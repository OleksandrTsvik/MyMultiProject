import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Icon } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import EmptyBlock from '../../components/EmptyBlock';
import Loading from '../../components/Loading';
import AddCategory from './AddCategory';
import CategoriesList from './CategoriesList';

export default observer(function CategoriesPage() {
  const { modalStore, dictionaryStore } = useStore();

  const { openModal } = modalStore;
  const {
    loadCategories, loadingCategories,
    categories, resetCategories
  } = dictionaryStore;

  useEffect(() => {
    loadCategories();

    return () => {
      resetCategories();
    }
  }, [loadCategories, resetCategories]);

  if (loadingCategories) {
    return <Loading content="Loading dictionary categories..." dimmer={false} />;
  }

  return (
    <>
      <div className="text-end mb-3">
        <Button onClick={() => openModal(<AddCategory />, { size: 'small' }, true)}>
          <Icon name="add" />
          Add category
        </Button>
      </div>
      {categories.size === 0
        ? <EmptyBlock />
        : <CategoriesList />
      }
    </>
  );
});