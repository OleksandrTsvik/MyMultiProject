import { observer } from 'mobx-react-lite';

import { useStore } from '../../../app/stores/store';
import CategoryModalForm, { DictionaryCategoryForm } from './CategoryModalForm';

export default observer(function AddCategory() {
  const { dictionaryStore } = useStore();
  const { createCategory } = dictionaryStore;

  function handleSubmit(createdCategory: DictionaryCategoryForm) {
    return createCategory(createdCategory);
  }

  return (
    <>
      <CategoryModalForm
        title="Add a new category"
        textForSubmitBtn="Add"
        initialValues={{
          title: '',
          language: ''
        }}
        onSubmit={handleSubmit}
      />
    </>
  );
});