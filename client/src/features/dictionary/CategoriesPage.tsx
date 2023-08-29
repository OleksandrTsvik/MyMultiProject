import { SyntheticEvent, useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { useNavigate } from 'react-router-dom';
import { Button, Flag, FlagNameValues, Icon, Table } from 'semantic-ui-react';

import { DictionaryCategory } from '../../app/models/dictionary';
import { useStore } from '../../app/stores/store';
import ModalConfirm from '../../app/common/modals/ModalConfirm';
import EmptyBlock from '../../components/EmptyBlock';
import Loading from '../../components/Loading';
import AddCategory from './AddCategory';
import EditCategory from './EditCategory';
import StatusLabel from './StatusLabel';

export default observer(function CategoriesPage() {
  const navigate = useNavigate();

  const { modalStore, dictionaryStore } = useStore();

  const { openModal } = modalStore;
  const {
    loadCategories, loadingCategories,
    categories, categoriesArray,
    deleteCategory, resetCategories
  } = dictionaryStore;

  useEffect(() => {
    loadCategories();

    return () => {
      resetCategories();
    }
  }, [loadCategories, resetCategories]);

  function handleOpenEditCategory(event: SyntheticEvent, category: DictionaryCategory) {
    event.stopPropagation();

    openModal(<EditCategory category={category} />, { size: 'small' }, true);
  }

  function handleOpenDeleteCategory(event: SyntheticEvent, category: DictionaryCategory) {
    event.stopPropagation();

    openModal(
      <ModalConfirm
        content={`Delete the category "${category.title}".`}
        onConfirm={() => deleteCategory(category.id)}
      />,
      {},
      true
    );
  }

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
        : <div className="table-responsive">
          <Table selectable unstackable>
            <Table.Body>
              {categoriesArray.map((category) => (
                <Table.Row
                  key={category.id}
                  className="cursor-pointer"
                  verticalAlign="top"
                  onClick={() => navigate(`/dictionary/categories/${category.id}`)}
                >
                  <Table.Cell collapsing>
                    <Icon name="block layout" />
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <Flag name={category.language.toLowerCase() as FlagNameValues} />
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <StatusLabel counter content={category.countItems} />
                  </Table.Cell>
                  <Table.Cell>
                    {category.title}
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <div className="d-flex align-self-center gap-2">
                      <Button
                        className="m-0"
                        icon="pencil alternate"
                        color="blue"
                        onClick={(event) => handleOpenEditCategory(event, category)}
                      />
                      <Button
                        className="m-0"
                        icon="trash alternate"
                        color="red"
                        onClick={(event) => handleOpenDeleteCategory(event, category)}
                      />
                    </div>
                  </Table.Cell>
                </Table.Row>
              ))}
            </Table.Body>
          </Table>
        </div>
      }
    </>
  );
});