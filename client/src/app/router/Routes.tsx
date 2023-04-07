import { RouteObject, createBrowserRouter } from 'react-router-dom';

import App from '../layout/App';
import HomePage from '../../features/home/HomePage';
import DutyDashboard from '../../features/duties/dashboard/DutyDashboard';
import NotFoundPage from '../../features/404/NotFoundPage';

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            { path: '', element: <HomePage /> },
            { path: 'tasks', element: <DutyDashboard /> },
            { path: '*', element: <NotFoundPage /> }
        ]
    }
];

export const router = createBrowserRouter(routes);