import React, { Fragment } from 'react';
import { Grid } from 'semantic-ui-react';
import { Duty } from '../../../app/models/duty';
import DutyListItem from './DutyListItem';

interface Props {
    duties: Duty[];
    openEditMode: (id: string) => void;
}

export default function DutyList({ duties, openEditMode }: Props) {
    if (duties.length === 0) {
        return <Fragment />;
    }

    return (
        <Grid>
            {duties.map(duty => (
                <DutyListItem key={duty.id}
                    duty={duty}
                    openEditMode={openEditMode}
                />
            ))}
        </Grid>
    );
}