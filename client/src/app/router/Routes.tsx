import { Navigate, RouteObject, createBrowserRouter } from 'react-router-dom';

import App from '../layout/App';
import HomePage from '../../features/home/HomePage';
import DutyDashboard from '../../features/duties/dashboard/DutyDashboard';
import NotFoundPage from '../../features/errors/NotFoundPage';
import TranslateKeyboardPage from '../../features/translate/TranslateKeyboardPage';
import TestErrorsPage from '../../features/errors/TestErrorPage';
import ServerErrorPage from '../../features/errors/ServerErrorPage';

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            { path: '', element: <HomePage /> },
            { path: 'tasks', element: <DutyDashboard /> },
            {
                path: 'translate',
                children: [
                    { path: 'language', element: <HomePage /> },
                    { path: 'keyboard', element: <TranslateKeyboardPage /> }
                ]
            },
            { path: 'games', element: <>Games</> },
            { path: 'errors', element: <TestErrorsPage /> },
            { path: 'server-error', element: <ServerErrorPage /> },
            { path: 'not-found', element: <NotFoundPage /> },
            { path: '*', element: <Navigate to="/not-found" replace /> }
        ]
    }
];

export const router = createBrowserRouter(routes);