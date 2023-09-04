import { Navigate, useParams } from 'react-router-dom';

import { useStore } from '../../../app/stores/store';
import CategoryItemForm, { DictionaryItemForm } from './CategoryItemForm';

export default function AddCategoryItemPage() {
  const { categoryId } = useParams();

  const { dictionaryStore } = useStore();
  const { createItem } = dictionaryStore;

  function handleSubmit(createdItem: DictionaryItemForm) {
    if (!categoryId) {
      return;
    }

    return createItem({
      ...createdItem,
      categoryId
    });
  }

  if (!categoryId) {
    return <Navigate to="/not-found" replace />;
  }

  return (
    <>
      <CategoryItemForm
        title="Add a new category item"
        textForSubmitBtn="Add"
        linkToBack={`/dictionary/categories/${categoryId}`}
        initialValues={{
          text: '',
          translation: ''
        }}
        onSubmit={handleSubmit}
      />
    </>
  );
}