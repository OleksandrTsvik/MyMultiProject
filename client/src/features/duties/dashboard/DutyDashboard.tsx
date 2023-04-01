import { observer } from 'mobx-react-lite';
import { Segment } from 'semantic-ui-react';

import { useStore } from '../../../app/stores/store';
import DutyList from './DutyList';
import DutyCreate from './DutyCreate';
import DutyModalCreate from '../modal/DutyModalCreate';
import DutyModalDelete from '../modal/DutyModalDelete';
import DutyModalEdit from '../modal/DutyModalEdit';

export default observer(function DutyDashboard() {
    const { dutyStore } = useStore();
    const { dutiesCompleted, dutiesNotCompleted, countCompleted } = dutyStore;

    return (
        <>
            <DutyCreate />
            <DutyList duties={dutiesNotCompleted} />
            <Segment inverted color='green' textAlign='center' size='big'>
                The latter are completed ({countCompleted})
            </Segment>
            <DutyList duties={dutiesCompleted} />
            <DutyModalCreate />
            <DutyModalEdit />
            <DutyModalDelete />
        </>
    );
});