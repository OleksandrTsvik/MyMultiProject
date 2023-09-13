import { observer } from 'mobx-react-lite';
import { Table } from 'semantic-ui-react';
import { DragDropContext, Draggable, DropResult, Droppable, ResponderProvided } from 'react-beautiful-dnd';

import { useStore } from '../../../app/stores/store';
import ListItem from './ListItem';

export default observer(function ItemsList() {
  const { dictionaryStore } = useStore();
  const { itemsSortByPosition, changeItemPositions } = dictionaryStore;

  function handleDragEnd(result: DropResult, provided: ResponderProvided) {
    if (!result.destination) {
      return;
    }

    changeItemPositions(result.source.index, result.destination.index);
  }

  return (
    <DragDropContext onDragEnd={handleDragEnd}>
      <div className="table-responsive">
        <Table celled striped selectable unstackable>
          <Droppable droppableId="items">
            {(provided, snapshot) => (
              <tbody
                ref={provided.innerRef}
                {...provided.droppableProps}
              >
                {itemsSortByPosition.map((item, index) => (
                  <Draggable
                    key={item.id}
                    draggableId={`items-${item.id}`}
                    index={index}
                  >
                    {(provided, snapshot) => (
                      <ListItem
                        item={item}
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