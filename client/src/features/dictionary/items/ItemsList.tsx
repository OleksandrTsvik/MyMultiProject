import { observer } from 'mobx-react-lite';
import { Table } from 'semantic-ui-react';

import { useStore } from '../../../app/stores/store';
import ListItem from './ListItem';

export default observer(function ItemsList() {
  const { dictionaryStore } = useStore();
  const { itemsSortByPosition } = dictionaryStore;

  return (
    <div className="table-responsive">
      <Table celled striped selectable unstackable>
        <Table.Body>
          {itemsSortByPosition.map((item) => (
            <ListItem key={item.id} item={item} />
          ))}
        </Table.Body>
      </Table>
    </div>
  );
});