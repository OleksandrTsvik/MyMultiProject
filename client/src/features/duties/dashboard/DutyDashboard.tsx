import React from 'react';
import { Segment } from 'semantic-ui-react';
import { Duty } from '../../../app/models/duty';
import DutyModalCreate from '../modal/DutyModalCreate';
import DutyModalDelete from '../modal/DutyModalDelete';
import DutyModalEdit from '../modal/DutyModalEdit';
import DutyCreate from './DutyCreate';
import DutyList from './DutyList';

interface Props {
    duties: Duty[];
    setDuties: React.Dispatch<React.SetStateAction<Duty[]>>;
    selectedDuty: Duty | undefined;
    countCompletedDuties: number;
    countNotCompletedDuties: number;
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

export default function DutyDashboard({ duties, setDuties, selectedDuty,
    countCompletedDuties, countNotCompletedDuties,
    createMode, openCreateMode, closeCreateMode,
    editMode, openEditMode, closeEditMode,
    deleteMode, openDeleteMode, closeDeleteMode }: Props) {
    return (
        <>
            <DutyCreate
                countNotCompletedDuties={countNotCompletedDuties}
                openCreateMode={openCreateMode}
            />

            <DutyList
                duties={duties.filter(duty => !duty.isCompleted)}
                openEditMode={openEditMode}
                openDeleteMode={openDeleteMode}
            />

            <Segment inverted color='green' textAlign='center' size='big'>
                The latter are completed ({countCompletedDuties})
            </Segment>

            <DutyList
                duties={duties.filter(duty => duty.isCompleted)}
                openEditMode={openEditMode}
                openDeleteMode={openDeleteMode}
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
                    deleteMode={deleteMode}
                    closeDeleteMode={closeDeleteMode}
                />
            }
        </>
    );
}