import React, { useEffect, useState } from 'react';
import './App.css';
import axios from 'axios';
import { Header } from 'semantic-ui-react';
import List from 'semantic-ui-react/dist/commonjs/elements/List';

function App() {
  const [duties, setDuties] = useState([]);

  useEffect(() => {
    axios.get("http://localhost:5000/api/duties")
      .then(response => {
        console.log(response);
        setDuties(response.data);
      });
  }, []);

  return (
    <div>
      <Header as="h2" icon="tasks" content="Duties" />
      <List>
        {duties.map((duty: any) => (
          <List.Item key={duty.id}>
            {duty.title}
          </List.Item>
        ))}
      </List>
    </div>
  );
}

export default App;
