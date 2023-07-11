import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Outlet, ScrollRestoration } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';

import { useStore } from '../stores/store';
import ModalContainer from '../common/modals/ModalContainer';
import Loading from '../../components/Loading';
import NavBar from './NavBar';
import Footer from './Footer';

export default observer(function App() {
    const { commonStore, userStore } = useStore();

    useEffect(() => {
        if (commonStore.token) {
            userStore.getUser()
                .finally(() => commonStore.setAppLoaded());
        } else {
            commonStore.setAppLoaded();
        }
    }, [commonStore, userStore]);

    if (!commonStore.appLoaded) {
        return <Loading content="Loading app..." />;
    }

    return (
        <>
            <ScrollRestoration />
            <NavBar />
            <main className="wrapper">
                <Outlet />
            </main>
            <Footer />
            <ModalContainer />
            <ToastContainer position="bottom-right" theme="colored" />
        </>
    );
});