import { observer } from 'mobx-react-lite';

import { DictionaryCategory } from '../../../app/models/dictionary';
import { useStore } from '../../../app/stores/store';
import CategoryModalForm, { DictionaryCategoryForm } from './CategoryModalForm';

interface Props {
  category: DictionaryCategory;
}

export default observer(function EditCategory({ category }: Props) {
  const { dictionaryStore } = useStore();
  const { editCategory } = dictionaryStore;

  function handleSubmit(editedCategory: DictionaryCategoryForm) {
    return editCategory({
      ...editedCategory,
      id: category.id
    });
  }

  return (
    <>
      <CategoryModalForm
        title="Update category"
        textForSubmitBtn="Update"
        initialValues={category}
        onSubmit={handleSubmit}
      />
    </>
  );
});