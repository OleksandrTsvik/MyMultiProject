import { useEffect, useState } from 'react';
import { Navigate, useParams } from 'react-router-dom';

import { DictionaryItem } from '../../../app/models/dictionary';
import { useStore } from '../../../app/stores/store';
import agent from '../../../app/api/agent';
import Loading from '../../../components/Loading';
import CategoryItemForm, { DictionaryItemForm } from './CategoryItemForm';

export default function EditCategoryItemPage() {
  const { categoryId, itemId } = useParams();

  const { dictionaryStore } = useStore();
  const { editItem } = dictionaryStore;

  const [item, setItem] = useState<DictionaryItem | undefined>();
  const [loadingItem, setLoadingItem] = useState<boolean>(true);

  useEffect(() => {
    if (itemId) {
      agent.DictionaryItems.details(itemId)
        .then((data) => setItem(data))
        .finally(() => setLoadingItem(false));
    } else {
      setLoadingItem(false);
    }
  }, [itemId]);

  function handleSubmit(editedItem: DictionaryItemForm) {
    if (!itemId) {
      return;
    }

    return editItem({
      ...editedItem,
      id: itemId
    });
  }

  if (loadingItem) {
    return <Loading content="Loading item..." />;
  }

  if (!categoryId || !itemId || !item) {
    return <Navigate to="/not-found" replace />;
  }

  return (
    <>
      <CategoryItemForm
        title="Update category item"
        textForSubmitBtn="Update"
        linkToBack={`/dictionary/categories/${categoryId}`}
        initialValues={item}
        onSubmit={handleSubmit}
      />
    </>
  );
}