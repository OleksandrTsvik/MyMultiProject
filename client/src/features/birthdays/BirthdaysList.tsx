import { observer } from 'mobx-react-lite';
import { Table } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import BirthdayListItem from './BirthdayListItem';

export default observer(function BirthdaysList() {
  const { birthdayStore } = useStore();
  const { birthdaysSortByDaysUntilBirthday } = birthdayStore;

  return (
    <div className="table-responsive">
      <Table celled structured unstackable>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell></Table.HeaderCell>
            <Table.HeaderCell>Name</Table.HeaderCell>
            <Table.HeaderCell textAlign="center">Age</Table.HeaderCell>
            <Table.HeaderCell collapsing>Days until birthday</Table.HeaderCell>
            <Table.HeaderCell textAlign="center">Date</Table.HeaderCell>
            <Table.HeaderCell></Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {birthdaysSortByDaysUntilBirthday.map((birthday, index) => (
            <BirthdayListItem
              key={birthday.id}
              birthday={birthday}
              index={index}
            />
          ))}
        </Table.Body>
      </Table>
    </div>
  );
});
