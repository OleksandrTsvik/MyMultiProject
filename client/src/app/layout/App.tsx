import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';

import { useStore } from '../stores/store';
import Loading from '../../components/Loading';
import DutyDashboard from '../../features/duties/dashboard/DutyDashboard';
import NavBar from './NavBar';
import Footer from './Footer';

export default observer(function App() {
  const { dutyStore } = useStore();

  useEffect(() => {
    dutyStore.loadDuties();
  }, [dutyStore]);

  if (dutyStore.loadingInitial) {
    return <Loading content="Loading app..." />;
  }

  return (
    <>
      <NavBar />
      <main className="wrapper">
        <DutyDashboard />
      </main>
      <Footer />
    </>
  );
});