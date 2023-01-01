import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Duty } from '../models/duty';
import NavBar from './NavBar';
import DutyDashboard from '../../features/duties/dashboard/DutyDashboard';
import Footer from './Footer';

function App() {
  const [duties, setDuties] = useState<Duty[]>([]);
  const [countCompletedDuties, setCountCompletedDuties] = useState<number>(0);
  const [countNotCompletedDuties, setCountNotCompletedDuties] = useState<number>(0);
  const [selectedDuty, setSelectedDuty] = useState<Duty | undefined>(undefined);

  const [createMode, setCreateMode] = useState<boolean>(false);
  const [editMode, setEditMode] = useState<boolean>(false);
  const [deleteMode, setDeleteMode] = useState<boolean>(false);

  useEffect(() => {
    axios.get<Duty[]>("http://localhost:5000/api/duties")
      .then(response => {
        setDuties(response.data);
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
