import React from 'react';
import { observer } from 'mobx-react-lite';
import { Segment } from 'semantic-ui-react';

import { Duty } from '../../../app/models/duty';
import { useStore } from '../../../app/stores/store';
import DutyList from './DutyList';
import DutyCreate from './DutyCreate';
import DutyModalCreate from '../modal/DutyModalCreate';
import DutyModalDelete from '../modal/DutyModalDelete';
import DutyModalEdit from '../modal/DutyModalEdit';

interface Props {
    duties: Duty[];
    setDuties: React.Dispatch<React.SetStateAction<Duty[]>>;
    selectedDuty: Duty | undefined;
    createMode: boolean;
    openCreateMode: () => void;
    closeCreateMode: () => void;
    editMode: boolean;
    openEditMode: (id: string) => void;
    closeEditMode: () => void;
    deleteMode: boolean;
    openDeleteMode: (id: string) => void;
    closeDeleteMode: (id?: string) => void;
}

export default observer(function DutyDashboard(
    {
        duties, setDuties,
        createMode, openCreateMode, closeCreateMode,
        editMode, openEditMode, closeEditMode,
        openDeleteMode, closeDeleteMode
    }: Props
) {
    const { dutyStore } = useStore();
    const { countCompleted, selectedDuty, deleteMode } = dutyStore;

    return (
        <>
            <DutyCreate
                openCreateMode={openCreateMode}
            />

            <DutyList
                duties={duties.filter(duty => !duty.isCompleted)}
                openEditMode={openEditMode}
            />

            <Segment inverted color='green' textAlign='center' size='big'>
                The latter are completed ({countCompleted})
            </Segment>

            <DutyList
                duties={duties.filter(duty => duty.isCompleted)}
                openEditMode={openEditMode}
            />

            {selectedDuty && editMode &&
                <DutyModalEdit
                    duty={selectedDuty}
                    setDuties={setDuties}
                    editMode={editMode}
                    closeEditMode={closeEditMode}
                />
            }

            <DutyModalCreate
                setDuties={setDuties}
                createMode={createMode}
                closeCreateMode={closeCreateMode}
            />

            {selectedDuty && deleteMode &&
                <DutyModalDelete
                    duty={selectedDuty}
                />
            }
        </>
    );
});