import { Navigate, RouteObject, createBrowserRouter } from 'react-router-dom';

import App from '../layout/App';
import HomePage from '../../features/home/HomePage';
import DutyDashboard from '../../features/duties/dashboard/DutyDashboard';
import NotFoundPage from '../../features/errors/NotFoundPage';
import TranslateKeyboardPage from '../../features/translate/TranslateKeyboardPage';
import TestErrorsPage from '../../features/errors/TestErrorPage';
import ServerErrorPage from '../../features/errors/ServerErrorPage';
import ProfilePage from '../../features/profiles/ProfilePage';
import RequireAuth from './RequireAuth';
import DictionaryPage from '../../features/dictionary/DictionaryPage';
import CategoriesPage from '../../features/dictionary/CategoriesPage';
import RulesPage from '../../features/dictionary/RulesPage';
import CategoryPage from '../../features/dictionary/CategoryPage';
import AddRulePage from '../../features/dictionary/AddRulePage';
import EditRulePage from '../../features/dictionary/EditRulePage';
import RuleItemPage from '../../features/dictionary/RuleItemPage';
import AddCategoryItemPage from '../../features/dictionary/AddCategoryItemPage';
import EditCategoryItemPage from '../../features/dictionary/EditCategoryItemPage';

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            { index: true, element: <HomePage /> },
            {
                element: <RequireAuth />,
                children: [
                    { path: 'tasks', element: <DutyDashboard /> },
                    {
                        path: 'dictionary',
                        element: <DictionaryPage />,
                        children: [
                            { index: true, element: <Navigate to="/dictionary/categories" replace /> },
                            { path: 'categories', element: <CategoriesPage /> },
                            { path: 'rules', element: <RulesPage /> }
                        ]
                    },
                    {
                        path: 'dictionary',
                        children: [
                            { path: 'categories/:categoryId', element: <CategoryPage /> },
                            { path: 'categories/:categoryId/item/add', element: <AddCategoryItemPage /> },
                            { path: 'categories/:categoryId/item/edit/:itemId', element: <EditCategoryItemPage /> },
                            { path: 'rules/add', element: <AddRulePage /> },
                            { path: 'rules/edit/:ruleId', element: <EditRulePage /> },
                            { path: 'rules/:ruleId', element: <RuleItemPage /> }
                        ]
                    }
                ]
            },
            {
                path: 'translate',
                children: [
                    { index: true, element: <Navigate to="/translate/language" replace /> },
                    { path: 'language', element: <HomePage /> },
                    { path: 'keyboard', element: <TranslateKeyboardPage /> }
                ]
            },
            { path: 'profiles/:username', element: <ProfilePage /> },
            { path: 'games', element: <>Games</> },
            { path: 'errors', element: <TestErrorsPage /> },
            { path: 'server-error', element: <ServerErrorPage /> },
            { path: 'not-found', element: <NotFoundPage /> },
            { path: '*', element: <Navigate to="/not-found" replace /> }
        ]
    }
];

export const router = createBrowserRouter(routes);