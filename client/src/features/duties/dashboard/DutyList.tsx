import { Fragment } from 'react';
import { Grid } from 'semantic-ui-react';

import { Duty } from '../../../app/models/duty';
import DutyListItem from './DutyListItem';

interface Props {
    duties: Duty[];
}

export default function DutyList({ duties }: Props) {
    if (duties.length === 0) {
        return <Fragment />;
    }

    return (
        <Grid>
            {duties.map(duty => (
                <DutyListItem key={duty.id} duty={duty} />
            ))}
        </Grid>
    );
}