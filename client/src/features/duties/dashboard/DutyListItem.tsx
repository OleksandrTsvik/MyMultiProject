import { DragEvent, MouseEvent } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Card, Grid, Icon, Popup } from 'semantic-ui-react';

import { Duty } from '../../../app/models/duty';
import { useStore } from '../../../app/stores/store';
import dateFormat from '../../../app/utils/dateFormat';

interface Props {
    duty: Duty;
    draggable?: boolean;
    onMouseDownDraggableElem: (event: MouseEvent<HTMLDivElement>) => void;
    onMouseUpDraggableElem: (event: MouseEvent<HTMLDivElement>) => void;
    onDragStart: (event: DragEvent<HTMLDivElement>, duty: Duty) => void;
    onDragEnd: (event: DragEvent<HTMLDivElement>) => void;
    onDragOver: (event: DragEvent<HTMLDivElement>, duty: Duty) => void;
}

export default observer(function DutyListItem(
    {
        duty,
        draggable,
        onMouseDownDraggableElem,
        onMouseUpDraggableElem,
        onDragStart,
        onDragEnd,
        onDragOver
    }: Props
) {
    const { dutyStore } = useStore();
    const { updateIsCompletedDuty, getIsLoading, openEditMode, openDeleteMode } = dutyStore;

    const style = {
        backgroundColor: duty.backgroundColor,
        color: duty.fontColor,
        borderColor: duty.fontColor
    };

    const dragAndDrop = draggable
        ? {
            className: 'rounded draggable__item',
            onDragStart: (e: DragEvent<HTMLDivElement>) => onDragStart(e, duty),
            onDragEnd: onDragEnd,
            onDragOver: (e: DragEvent<HTMLDivElement>) => onDragOver(e, duty)
        }
        : {
            className: 'rounded'
        };

    return (
        <Grid.Column
            mobile={16}
            tablet={8}
            computer={4}
            {...dragAndDrop}
        >
            <Card fluid style={style}>
                <Card.Content>
                    {!duty.isCompleted && draggable
                        ? <Card.Header
                            style={style}
                            className='d-flex justify-content-between'
                        >
                            <div>{duty.title}</div>
                            <div
                                className='draggable'
                                onMouseDown={onMouseDownDraggableElem}
                                onMouseUp={onMouseUpDraggableElem}
                            >
                                <Icon name='grid layout' />
                            </div>
                        </Card.Header>
                        : <Card.Header style={style}>{duty.title}</Card.Header>
                    }
                </Card.Content>
                <Card.Content style={style}>
                    <Card.Description style={style}>{duty.description}</Card.Description>
                </Card.Content>
                <Card.Content style={style}>
                    <Card.Meta style={style}>
                        <Popup
                            content='date create'
                            position='bottom left'
                            trigger={
                                <div>
                                    <Icon name='calendar alternate' />&ensp;
                                    {dateFormat(duty.dateCreation)}
                                </div>
                            }
                        />
                    </Card.Meta>
                </Card.Content>
                <Card.Content
                    style={style}
                    className='d-flex flex-row-reverse gap-2'
                >
                    <Button circular
                        icon='tint'
                        color='purple'
                        className='m-0'
                    />
                    <Button inverted circular
                        icon='trash alternate'
                        color='red'
                        className='m-0'
                        onClick={() => openDeleteMode(duty.id)}
                    />
                    <Button inverted circular
                        icon='pencil alternate'
                        color='blue'
                        className='m-0'
                        onClick={() => openEditMode(duty.id)}
                    />
                    {duty.isCompleted
                        ? <Button inverted circular
                            icon='cancel'
                            color='orange'
                            className='m-0'
                            loading={getIsLoading(duty.id)}
                            disabled={getIsLoading(duty.id)}
                            onClick={() => updateIsCompletedDuty(duty, false)}
                        />
                        : <Button inverted circular
                            icon='check'
                            color='green'
                            className='m-0'
                            loading={getIsLoading(duty.id)}
                            disabled={getIsLoading(duty.id)}
                            onClick={() => updateIsCompletedDuty(duty, true)}
                        />
                    }
                </Card.Content>
            </Card>
        </Grid.Column>
    );
});