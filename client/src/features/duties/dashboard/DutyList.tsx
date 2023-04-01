import { Grid } from 'semantic-ui-react';

import { Duty } from '../../../app/models/duty';
import DutyListItem from './DutyListItem';
import EmptyBlock from '../../../components/EmptyBlock';

interface Props {
    duties: Duty[];
}

export default function DutyList({ duties }: Props) {
    if (duties.length === 0) {
        return <EmptyBlock />;
    }

    return (
        <Grid>
            {duties.map(duty => (
                <DutyListItem key={duty.id} duty={duty} />
            ))}
        </Grid>
    );
}