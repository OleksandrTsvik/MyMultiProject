import ReactDOM from 'react-dom/client';

import App from './app/layout/App';
import { store, StoreContext } from './app/stores/store';

import 'semantic-ui-css/semantic.min.css'
import './app/layout/styles.scss';
import './app/layout/short-bootstrap.css';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <StoreContext.Provider value={store}>
    <App />
  </StoreContext.Provider>
);