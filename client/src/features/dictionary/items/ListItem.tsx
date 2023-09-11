import { observer } from 'mobx-react-lite';
import { Link, useParams } from 'react-router-dom';
import { Button, Icon, Table } from 'semantic-ui-react';

import { DictionaryItem } from '../../../app/models/dictionary';
import { useStore } from '../../../app/stores/store';
import ModalConfirm from '../../../app/common/modals/ModalConfirm';
import StatusLabel from '../StatusLabel';

interface Props {
  item: DictionaryItem;
}

export default observer(function ItemsList({ item }: Props) {
  const { categoryId } = useParams();

  const { modalStore, dictionaryStore } = useStore();

  const { openModal } = modalStore;
  const { deleteItem } = dictionaryStore;

  function handleOpenDeleteCategoryItem() {
    openModal(
      <ModalConfirm
        content={<>
          <div>Delete the category item:</div>
          <div dangerouslySetInnerHTML={{ __html: item.text }} />
        </>}
        onConfirm={() => deleteItem(item.id)}
      />,
      {},
      true
    );
  }

  return (
    <Table.Row verticalAlign="top">
      <Table.Cell collapsing>
        <Icon name="block layout" />
      </Table.Cell>
      <Table.Cell>
        <div dangerouslySetInnerHTML={{ __html: item.text }} />
      </Table.Cell>
      <Table.Cell>
        <div dangerouslySetInnerHTML={{ __html: item.translation }} />
      </Table.Cell>
      <Table.Cell collapsing>
        <StatusLabel className="m-0" status={item.status} />
      </Table.Cell>
      <Table.Cell collapsing>
        <div className="d-flex gap-2">
          <Button
            className="m-0"
            icon="pencil alternate"
            color="blue"
            as={Link} to={`/dictionary/categories/${categoryId}/item/edit/${item.id}`}
          />
          <Button
            className="m-0"
            icon="trash alternate"
            color="red"
            onClick={handleOpenDeleteCategoryItem}
          />
        </div>
      </Table.Cell>
    </Table.Row>
  );
});