import { useParams } from 'react-router-dom';

import CategoryItemForm from './CategoryItemForm';

export default function AddCategoryItemPage() {
  const { categoryId, itemId } = useParams();

  return (
    <>
      <CategoryItemForm
        title="Add a new category item"
        textForSubmitBtn="Add"
        linkToBack={`/dictionary/categories/${categoryId}`}
      />
    </>
  );
}