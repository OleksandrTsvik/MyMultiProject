import { SyntheticEvent } from 'react';
import { observer } from 'mobx-react-lite';
import { useNavigate } from 'react-router-dom';
import { Button, Flag, Icon, Label, Table } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import ModalConfirm from '../../app/common/modals/ModalConfirm';
import EmptyBlock from '../../components/EmptyBlock';
import AddCategory from './AddCategory';
import EditCategory from './EditCategory';

export default observer(function CategoriesPage() {
  const navigate = useNavigate();

  const { modalStore } = useStore();
  const { openModal } = modalStore;

  function handleOpenEditCategory(event: SyntheticEvent) {
    event.stopPropagation();

    openModal(<EditCategory />, { size: 'small' }, true);
  }

  function handleOpenDeleteCategory(event: SyntheticEvent) {
    event.stopPropagation();

    openModal(
      <ModalConfirm
        content={`Delete the category ${'title'}.`}
        onConfirm={() => console.log('onConfirm Category')}
      />,
      {},
      true
    );
  }

  return (
    <>
      <div className="text-end mb-3">
        <Button onClick={() => openModal(<AddCategory />, { size: 'small' }, true)}>
          <Icon name="add" />
          Add category
        </Button>
      </div>
      {false
        ? <EmptyBlock />
        : <div className="table-responsive">
          <Table selectable unstackable>
            <Table.Body>
              {Array(15).fill(null).map((_, index) => (
                <Table.Row
                  key={index}
                  className="cursor-pointer"
                  verticalAlign="top"
                  onClick={() => navigate(`/dictionary/categories/${index}`)}
                >
                  <Table.Cell collapsing>
                    <Icon name="block layout" />
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <Flag name="america" />
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <Label horizontal color="teal">11</Label>
                  </Table.Cell>
                  <Table.Cell>
                    Category
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <div className="d-flex align-self-center gap-2">
                      <Button
                        className="m-0"
                        icon="pencil alternate"
                        color="blue"
                        onClick={handleOpenEditCategory}
                      />
                      <Button
                        className="m-0"
                        icon="trash alternate"
                        color="red"
                        onClick={handleOpenDeleteCategory}
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