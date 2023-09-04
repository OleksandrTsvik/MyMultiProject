import { useParams } from 'react-router-dom';

import CategoryItemForm from './CategoryItemForm';

export default function EditCategoryItemPage() {
  const { categoryId, itemId } = useParams();

  return (
    <>
      <CategoryItemForm
        title="Update category item"
        textForSubmitBtn="Update"
        linkToBack={`/dictionary/categories/${categoryId}`}
      />
    </>
  );
}