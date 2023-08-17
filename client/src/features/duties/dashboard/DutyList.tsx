import { DragEvent, MouseEvent, useState } from 'react';
import { Grid } from 'semantic-ui-react';

import { Duty } from '../../../app/models/duty';
import { useStore } from '../../../app/stores/store';
import DutyListItem from './DutyListItem';
import EmptyBlock from '../../../components/EmptyBlock';

interface Props {
  duties: Duty[];
  draggable?: boolean;
}

export default function DutyList({ duties, draggable }: Props) {
  const { dutyStore } = useStore();
  const { reorderDuties, reorderDutiesOnServer } = dutyStore;

  const [draggingDuty, setDraggingDuty] = useState<Duty | undefined>(undefined);

  if (duties.length === 0) {
    return <EmptyBlock />;
  }

  function handleMouseDownDraggableElem(event: MouseEvent<HTMLDivElement>) {
    let target = event.target as HTMLDivElement;
    target = target.closest('.duty__draggable') as HTMLDivElement;

    if (!target) {
      return;
    }

    target.draggable = true;
  }

  function handleMouseUpDraggableElem(event: MouseEvent<HTMLDivElement>) {
    let target = event.target as HTMLDivElement;
    target = target.closest('.duty__draggable') as HTMLDivElement;

    if (!target) {
      return;
    }

    target.draggable = false;
  }

  function handleDragStart(event: DragEvent<HTMLDivElement>, duty: Duty) {
    const target = event.target as HTMLDivElement;
    target.classList.add('dragging');

    setDraggingDuty(duty);
  }

  function handleDragEnd(event: DragEvent<HTMLDivElement>) {
    const target = event.target as HTMLDivElement;

    target.draggable = false;
    target.classList.remove('dragging');

    setDraggingDuty(undefined);
    reorderDutiesOnServer();
  }

  function handleDragOver(event: DragEvent<HTMLDivElement>, duty: Duty) {
    event.preventDefault();

    if (draggingDuty && draggingDuty.id !== duty.id) {
      reorderDuties(duty, draggingDuty);
    }
  }

  return (
    <Grid className="px-3 pt-2 pb-3">
      {duties.map(duty => (
        <DutyListItem
          key={duty.id}
          duty={duty}
          draggable={draggable}
          onMouseDownDraggableElem={handleMouseDownDraggableElem}
          onMouseUpDraggableElem={handleMouseUpDraggableElem}
          onDragStart={handleDragStart}
          onDragEnd={handleDragEnd}
          onDragOver={handleDragOver}
        />
      ))}
    </Grid>
  );
}