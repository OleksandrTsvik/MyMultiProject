import React from 'react';
import { Button, Card, Grid, Icon, Popup } from 'semantic-ui-react';
import { Duty } from '../../../app/models/duty';
import dateFormat from '../../../app/utils/dateFormat';

interface Props {
    duty: Duty;
    openEditMode: (id: string) => void;
    openDeleteMode: (id: string) => void;
}

export default function DutyListItem({ duty, openEditMode, openDeleteMode }: Props) {
    const style = {
        backgroundColor: duty.backgroundColor,
        color: duty.fontColor,
        borderColor: duty.fontColor
    };

    return (
        <Grid.Column mobile={16} tablet={8} computer={4}>
            <Card fluid style={style}>
                <Card.Content>
                    {duty.isCompleted
                        ? <Card.Header style={style}>{duty.title}</Card.Header>
                        : <Card.Header style={style} className='d-flex justify-content-between'>
                            <div>{duty.title}</div>
                            <div className='draggable'>
                                <Icon name='grid layout' />
                            </div>
                        </Card.Header>
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
                        />
                        : <Button inverted circular
                            icon='check'
                            color='green'
                            className='m-0'
                        />
                    }
                </Card.Content>
            </Card>
        </Grid.Column>
    );
}