import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Outlet } from 'react-router-dom';

import { useStore } from '../stores/store';
import Loading from '../../components/Loading';
import NavBar from './NavBar';
import Footer from './Footer';

export default observer(function App() {
  const { dutyStore } = useStore();
  const { duties, loadDuties, loadingInitial } = dutyStore;

  useEffect(() => {
    if (duties.size === 0) {
      loadDuties();
    }
  }, [duties.size, loadDuties]);

  if (loadingInitial) {
    return <Loading content="Loading tasks..." />;
  }

  return (
    <>
      <NavBar />
      <main className="wrapper">
        <Outlet />
      </main>
      <Footer />
    </>
  );
});