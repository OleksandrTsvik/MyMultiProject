import { useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Label, Loader, Menu, Tab } from 'semantic-ui-react';
import InfiniteScroll from 'react-infinite-scroller';

import { useStore } from '../../../app/stores/store';
import { PagingParams } from '../../../app/models/pagination';
import DutyModalCreate from '../modals/DutyModalCreate';
import DutyModalDelete from '../modals/DutyModalDelete';
import DutyModalEdit from '../modals/DutyModalEdit';
import DutyList from './DutyList';
import DutyCreate from './DutyCreate';

export default observer(function DutyDashboard() {
    const { dutyStore, userStore } = useStore();

    const {
        loadDuties, loadingInitial,
        dutiesSortByPosition, dutiesSortByDateCompletion,
        setChangeColorMode, setLoadingInitial,
        pagination, setPagingParams,
        setPredicate
    } = dutyStore;

    const { countCompletedDuties, countNotCompletedDuties } = userStore;

    const [loadingNext, setLoadingNext] = useState<boolean>(false);

    useEffect(() => {
        setPredicate('isCompleted', false);

        return () => {
            setLoadingInitial(true);
        };
    }, [setPredicate, setLoadingInitial]);

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
    }, [setChangeColorMode]);

    function handleGetNext() {
        setLoadingNext(true);

        if (pagination) {
            setPagingParams(new PagingParams(pagination.currentPage + 1, pagination.itemsPerPage));
        }

        loadDuties().then(() => setLoadingNext(false));
    }

    const panes = [
        {
            menuItem: (
                <Menu.Item
                    key="not-completed"
                    onClick={() => setPredicate('isCompleted', false)}
                >
                    Not completed
                    <Label color="red">{countNotCompletedDuties}</Label>
                </Menu.Item>
            ),
            render: () =>
                <Tab.Pane loading={loadingInitial && !loadingNext}>
                    <DutyCreate />
                    <InfiniteScroll
                        pageStart={0}
                        initialLoad={false}
                        hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
                        loadMore={handleGetNext}
                    >
                        <DutyList duties={dutiesSortByPosition} draggable />
                    </InfiniteScroll>
                    <Loader
                        content="Loading more tasks..."
                        active={loadingNext}
                        inline="centered"
                    />
                </Tab.Pane>
        },
        {
            menuItem: (
                <Menu.Item
                    key="completed"
                    onClick={() => setPredicate('isCompleted', true)}
                >
                    Completed
                    <Label color="green">{countCompletedDuties}</Label>
                </Menu.Item>
            ),
            render: () =>
                <Tab.Pane loading={loadingInitial && !loadingNext}>
                    <InfiniteScroll
                        pageStart={0}
                        initialLoad={false}
                        hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
                        loadMore={handleGetNext}
                    >
                        <DutyList duties={dutiesSortByDateCompletion} />
                    </InfiniteScroll>
                    <Loader
                        content="Loading more tasks..."
                        active={loadingNext}
                        inline="centered"
                    />
                </Tab.Pane>
        }
    ];

    return (
        <>
            <Tab panes={panes} />
            <DutyModalCreate />
            <DutyModalEdit />
            <DutyModalDelete />
        </>
    );
});