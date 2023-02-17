import React, { useEffect, useState } from 'react';
import { Duty } from '../models/duty';
import NavBar from './NavBar';
import DutyDashboard from '../../features/duties/dashboard/DutyDashboard';
import Footer from './Footer';
import LoadingComponent from './LoadingComponent';
import { useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';

function App() {
  const { dutyStore } = useStore();

  const [duties, setDuties] = useState<Duty[]>([]);
  const [selectedDuty, setSelectedDuty] = useState<Duty | undefined>(undefined);

  const [createMode, setCreateMode] = useState<boolean>(false);
  const [editMode, setEditMode] = useState<boolean>(false);
  const [deleteMode, setDeleteMode] = useState<boolean>(false);

  useEffect(() => {
    dutyStore.loadDuties();
  }, [dutyStore]);

  function handleCreateModeOpen() {
    setCreateMode(true);
  }

  function handleCreateModeClose() {
    setCreateMode(false);
  }

  function handleSelectDuty(id: string) {
    setSelectedDuty(duties.find(duty => duty.id === id));
  }

  function handleCancelSelectedDuty() {
    setSelectedDuty(undefined);
  }

  function handleEditModeOpen(id: string) {
    setEditMode(true);
    handleSelectDuty(id);
  }

  function handleEditModeClose() {
    setEditMode(false);
    handleCancelSelectedDuty();
  }

  function handleDeleteModeOpen(id: string) {
    setDeleteMode(true);
    handleSelectDuty(id);
  }

  function handleDeleteModeClose(id?: string) {
    if (id) {
      setDuties(duties.filter(duty => duty.id !== id));
    }

    setDeleteMode(false);
    handleCancelSelectedDuty();
  }

  if (dutyStore.loadingInitial) {
    return (
      <LoadingComponent content='Loading app...' />
    );
  }

  return (
    <>
      <NavBar />
      <main>
        <DutyDashboard
          duties={dutyStore.duties}
          setDuties={setDuties}
          selectedDuty={selectedDuty}
          createMode={createMode}
          openCreateMode={handleCreateModeOpen}
          closeCreateMode={handleCreateModeClose}
          editMode={editMode}
          openEditMode={handleEditModeOpen}
          closeEditMode={handleEditModeClose}
          deleteMode={deleteMode}
          openDeleteMode={handleDeleteModeOpen}
          closeDeleteMode={handleDeleteModeClose}
        />
      </main>
      <Footer />
    </>
  );
}

export default observer(App);
