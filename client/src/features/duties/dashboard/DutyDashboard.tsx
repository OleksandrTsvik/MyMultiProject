import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Segment } from 'semantic-ui-react';

import { useStore } from '../../../app/stores/store';
import DutyModalCreate from '../modals/DutyModalCreate';
import DutyModalDelete from '../modals/DutyModalDelete';
import DutyModalEdit from '../modals/DutyModalEdit';
import DutyList from './DutyList';
import DutyCreate from './DutyCreate';
import DutyLoading from './DutyLoading';

export default observer(function DutyDashboard() {
    const { dutyStore } = useStore();
    const {
        duties, loadDuties, loadingInitial,
        dutiesCompletedSortByDateCompletion,
        dutiesNotCompletedSortByPosition,
        countCompleted, setChangeColorMode,
        setLoadingInitial
    } = dutyStore;

    useEffect(() => {
        if (duties.size === 0) {
            loadDuties();
        }

        return () => {
            if (duties.size === 0) {
                setLoadingInitial(true);
            }
        };
    }, [duties.size, loadDuties, setLoadingInitial]);

    useEffect(() => {
        function handleCloseChangeColorMode(event: MouseEvent) {
            const target = event.target as HTMLElement;

            if (!target.closest('.duty__collapsible')) {
                setChangeColorMode(false);
            }
        }

        document.addEventListener('click', handleCloseChangeColorMode);

        return () => {
            document.removeEventListener('click', handleCloseChangeColorMode);
        };
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    if (loadingInitial) {
        return <DutyLoading />;
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