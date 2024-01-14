import { observer } from 'mobx-react-lite';
import { Table } from 'semantic-ui-react';
import { format } from 'date-fns';

import { Birthday } from '../../app/models/birthday';
import EditBirthdayButton from './EditBirthdayButton';
import DeleteBirthdayButton from './DeleteBirthdayButton';

interface Props {
  birthday: Birthday;
  index: number;
}

export default observer(function BirthdayListItem({ birthday, index }: Props) {
  const rowSpan = birthday.note ? 2 : 1;

  const positive =
    1 < birthday.daysUntilBirthday && birthday.daysUntilBirthday < 7;
  const negative = birthday.daysUntilBirthday <= 1;

  return (
    <>
      <Table.Row positive={positive} negative={negative}>
        <Table.Cell
          collapsing
          rowSpan={rowSpan}
          bgcolor={index % 2 === 0 ? '#00b5ad' : null}
        />
        <Table.Cell>{birthday.fullName}</Table.Cell>
        <Table.Cell collapsing textAlign="center">
          {birthday.age}
        </Table.Cell>
        <Table.Cell collapsing textAlign="right">
          {birthday.daysUntilBirthday}
        </Table.Cell>
        <Table.Cell collapsing textAlign="center">
          {format(birthday.date, 'dd.MM.y')}
        </Table.Cell>
        <Table.Cell collapsing>
          <div className="d-flex align-self-center gap-2">
            <EditBirthdayButton birthday={birthday} />
            <DeleteBirthdayButton birthday={birthday} />
          </div>
        </Table.Cell>
      </Table.Row>
      {birthday.note && (
        <Table.Row positive={positive} negative={negative}>
          <Table.Cell colSpan={5}>{birthday.note}</Table.Cell>
        </Table.Row>
      )}
    </>
  );
});
