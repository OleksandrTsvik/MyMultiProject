import React from 'react';
import { Button, Card, Grid, Icon } from 'semantic-ui-react';
import { Duty } from '../../../app/models/duty';

interface Props {
    duty: Duty;
    openEditMode: (id: string) => void;
    openDeleteMode: (id: string) => void;
}

export default function DutyListItem({ duty, openEditMode, openDeleteMode }: Props) {
    return (
        <Grid.Column mobile={16} tablet={8} computer={4}>
            <Card fluid>
                <Card.Content>
                    {duty.isCompleted
                        ? <Card.Header>{duty.title}</Card.Header>
                        : <Card.Header className='d-flex justify-content-between'>
                            <div>{duty.title}</div>
                            <div className='draggable'>
                                <Icon name='grid layout' />
                            </div>
                        </Card.Header>
                    }
                </Card.Content>
                <Card.Content>
                    <Card.Description>{duty.description}</Card.Description>
                </Card.Content>
                <Card.Content>
                    <Card.Meta>
                        <Icon name='calendar alternate' />&ensp;
                        {duty.dateCreation}
                    </Card.Meta>
                </Card.Content>
                <Card.Content extra textAlign='right'>
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