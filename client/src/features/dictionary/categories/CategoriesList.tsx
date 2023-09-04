import { observer } from 'mobx-react-lite';
import { Table } from 'semantic-ui-react';
import { DragDropContext, Draggable, DropResult, Droppable, ResponderProvided } from 'react-beautiful-dnd';

import { useStore } from '../../../app/stores/store';
import CategoryListItem from './CategoryListItem';

export default observer(function CategoriesPage() {
  const { dictionaryStore } = useStore();
  const { categoriesSortByPosition, changePositions } = dictionaryStore;

  function handleDragEnd(result: DropResult, provided: ResponderProvided) {
    if (!result.destination) {
      return;
    }

    changePositions(result.source.index, result.destination.index);
  }

  return (
    <DragDropContext onDragEnd={handleDragEnd}>
      <div className="table-responsive">
        <Table selectable unstackable>
          <Droppable droppableId="categories">
            {(provided, snapshot) => (
              <tbody
                ref={provided.innerRef}
                {...provided.droppableProps}
              >
                {categoriesSortByPosition.map((category, index) => (
                  <Draggable
                    key={category.id}
                    draggableId={`categories-${category.id}`}
                    index={index}
                  >
                    {(provided, snapshot) => (
                      <CategoryListItem
                        category={category}
                        provided={provided}
                        snapshot={snapshot}
                      />
                    )}
                  </Draggable>
                ))}
                {provided.placeholder}
              </tbody>
            )}
          </Droppable>
        </Table>
      </div>
    </DragDropContext>
  );
});