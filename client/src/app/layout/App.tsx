import React, { useEffect, useState } from 'react';
import { Duty } from '../models/duty';
import NavBar from './NavBar';
import DutyDashboard from '../../features/duties/dashboard/DutyDashboard';
import Footer from './Footer';
import agent from '../api/agent';
import LoadingComponent from './LoadingComponent';

function App() {
  const [loading, setLoading] = useState<boolean>(true);

  const [duties, setDuties] = useState<Duty[]>([]);
  const [countCompletedDuties, setCountCompletedDuties] = useState<number>(0);
  const [countNotCompletedDuties, setCountNotCompletedDuties] = useState<number>(0);
  const [selectedDuty, setSelectedDuty] = useState<Duty | undefined>(undefined);

  const [createMode, setCreateMode] = useState<boolean>(false);
  const [editMode, setEditMode] = useState<boolean>(false);
  const [deleteMode, setDeleteMode] = useState<boolean>(false);

  useEffect(() => {
    agent.Duties.list()
      .then(response => {
        setDuties(response);
        setLoading(false);
      });
  }, []);

  useEffect(() => {
    setCountNotCompletedDuties(duties.filter(duty => !duty.isCompleted).length);
    setCountCompletedDuties(duties.filter(duty => duty.isCompleted).length);
  }, [duties]);

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

  if (loading) {
    return (
      <LoadingComponent content='Loading app...' />
    );
  }

  return (
    <>
      <NavBar countNotCompletedDuties={countNotCompletedDuties} />
      <main>
        <DutyDashboard
          duties={duties}
          setDuties={setDuties}
          selectedDuty={selectedDuty}
          countCompletedDuties={countCompletedDuties}
          countNotCompletedDuties={countNotCompletedDuties}
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

export default App;
