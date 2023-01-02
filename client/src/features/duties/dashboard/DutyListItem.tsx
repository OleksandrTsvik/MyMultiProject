import React from 'react';
import { Button, Card, Grid, Icon } from 'semantic-ui-react';
import { Duty } from '../../../app/models/duty';

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
                        <Icon name='calendar alternate' />&ensp;
                        {duty.dateCreation}
                    </Card.Meta>
                </Card.Content>
                <Card.Content style={style} textAlign='right'>
                    {duty.isCompleted
                        ? <Button inverted circular
                            icon='cancel'
                            color='orange' />
                        : <Button inverted circular
                            icon='check'
                            color='green' />
                    }
                    <Button inverted circular
                        icon='pencil alternate'
                        color='blue'
                        onClick={() => openEditMode(duty.id)}
                    />
                    <Button inverted circular
                        icon='trash alternate'
                        color='red'
                        onClick={() => openDeleteMode(duty.id)}
                    />
                    <Button circular
                        icon='tint'
                        color='purple'
                    />
                </Card.Content>
            </Card>
        </Grid.Column>
    );
}