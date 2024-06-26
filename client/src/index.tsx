import ReactDOM from 'react-dom/client';
import { RouterProvider } from 'react-router-dom';

import { store, StoreContext } from './app/stores/store';
import { router } from './app/router/Routes';

import 'semantic-ui-css/semantic.min.css';
import 'react-toastify/dist/ReactToastify.min.css';
import 'react-datepicker/dist/react-datepicker.css';
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';

import './app/layout/short-bootstrap.css';
import './app/layout/styles.scss';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <StoreContext.Provider value={store}>
    <RouterProvider router={router} />
  </StoreContext.Provider>
);