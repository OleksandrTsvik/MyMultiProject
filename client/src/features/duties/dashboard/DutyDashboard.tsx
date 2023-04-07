import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Segment } from 'semantic-ui-react';

import { useStore } from '../../../app/stores/store';
import DutyModalCreate from '../modal/DutyModalCreate';
import DutyModalDelete from '../modal/DutyModalDelete';
import DutyModalEdit from '../modal/DutyModalEdit';
import DutyList from './DutyList';
import DutyCreate from './DutyCreate';

export default observer(function DutyDashboard() {
    const { dutyStore } = useStore();
    const {
        dutiesCompletedSortByDateCompletion,
        dutiesNotCompletedSortByPosition,
        countCompleted, setChangeColorMode
    } = dutyStore;

    useEffect(() => {
        function handleCloseChangeColorMode(event: MouseEvent) {
            const target = event.target as HTMLElement;

            if (!target.closest('.duty__collapsible')) {
                setChangeColorMode(false);
            }
        }

        document.addEventListener('click', handleCloseChangeColorMode);

        return () => {
            document.removeEventListener('click', handleCloseChangeColorMode)
        };
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    return (
        <>
            <DutyCreate />
            <DutyList duties={dutiesNotCompletedSortByPosition} draggable />
            <Segment inverted color="green" textAlign="center" size="big">
                The latter are completed ({countCompleted})
            </Segment>
            <DutyList duties={dutiesCompletedSortByDateCompletion} />
            <DutyModalCreate />
            <DutyModalEdit />
            <DutyModalDelete />
        </>
    );
});