import { SyntheticEvent } from 'react';
import { observer } from 'mobx-react-lite';
import { useNavigate } from 'react-router-dom';
import { Button, Flag, FlagNameValues, Icon, Table } from 'semantic-ui-react';
import { DraggableProvided, DraggableStateSnapshot } from 'react-beautiful-dnd';

import { DictionaryCategory } from '../../app/models/dictionary';
import { useStore } from '../../app/stores/store';
import ModalConfirm from '../../app/common/modals/ModalConfirm';
import EditCategory from './EditCategory';
import StatusLabel from './StatusLabel';

interface Props {
  category: DictionaryCategory;
  provided: DraggableProvided;
  snapshot: DraggableStateSnapshot;
}

export default observer(function CategoryListItem(
  {
    category,
    provided,
    snapshot
  }: Props
) {
  const navigate = useNavigate();

  const { modalStore, dictionaryStore } = useStore();

  const { openModal } = modalStore;
  const { deleteCategory } = dictionaryStore;

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

  return (
    <tr
      ref={provided.innerRef}
      {...provided.draggableProps}
      className={"top aligned cursor-pointer" + (snapshot.isDragging ? ' item__dragging' : '')}
      onClick={() => navigate(`/dictionary/categories/${category.id}`)}
    >
      <Table.Cell
        collapsing
        {...provided.dragHandleProps}
      >
        <Icon name="block layout" />
      </Table.Cell>
      <Table.Cell collapsing>
        <Flag name={category.language.toLowerCase() as FlagNameValues} />
      </Table.Cell>
      <Table.Cell collapsing>
        <StatusLabel counter content={category.countItems} />
      </Table.Cell>
      <Table.Cell className={snapshot.isDragging ? 'w-100' : ''}>
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
    </tr>
  );
});