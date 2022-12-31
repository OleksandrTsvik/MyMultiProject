import React from 'react';
import { Grid, List } from 'semantic-ui-react';
import { Duty } from '../../../app/models/duty';

interface Props {
    duties: Duty[];
}

export default function DutyDashboard({ duties }: Props) {
    return (
        <Grid>
            <Grid.Column>
                <List>
                    {duties.map(duty => (
                        <List.Item key={duty.id}>
                            {duty.title}
                        </List.Item>
                    ))}
                </List>
            </Grid.Column>
        </Grid>
    );
}