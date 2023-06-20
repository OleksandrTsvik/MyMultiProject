import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Segment } from 'semantic-ui-react';

import { useStore } from '../../../app/stores/store';
import Loading from '../../../components/Loading';
import DutyModalCreate from '../modals/DutyModalCreate';
import DutyModalDelete from '../modals/DutyModalDelete';
import DutyModalEdit from '../modals/DutyModalEdit';
import DutyList from './DutyList';
import DutyCreate from './DutyCreate';

export default observer(function DutyDashboard() {
    const { dutyStore } = useStore();
    const {
        duties, loadDuties, loadingInitial,
        dutiesCompletedSortByDateCompletion,
        dutiesNotCompletedSortByPosition,
        countCompleted, setChangeColorMode
    } = dutyStore;

    useEffect(() => {
        if (duties.size === 0) {
            loadDuties();
        }
    }, [duties.size, loadDuties]);

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

    if (loadingInitial) {
        return <Loading content="Loading tasks..." />;
    }

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