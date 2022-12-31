import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Container, Header } from 'semantic-ui-react';
import { Duty } from '../models/duty';
import NavBar from './NavBar';
import DutyDashboard from '../../features/duties/dashboard/DutyDashboard';

function App() {
  const [duties, setDuties] = useState<Duty[]>([]);

  useEffect(() => {
    axios.get<Duty[]>("http://localhost:5000/api/duties")
      .then(response => {
        setDuties(response.data);
      });
  }, []);

  return (
    <>
      <NavBar />
      <Container>
        <Header as="h2" icon="tasks" content="Duties" style={{ marginTop: '3.5em' }} />
        <DutyDashboard duties={duties} />
      </Container>
    </>
  );
}

export default App;
