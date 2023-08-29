import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { NavLink, Outlet } from 'react-router-dom';
import { Container, Menu } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import Counter from '../../components/Counter';

export default observer(function DictionaryPage() {
  const { dictionaryStore } = useStore();
  const { loadQuantity, quantity, loadingQuantity } = dictionaryStore;

  useEffect(() => {
    loadQuantity();
  }, [loadQuantity]);

  return (
    <Container>
      <Menu
        pointing
        secondary
        fluid
        widths={2}
        color="teal"
      >
        <Menu.Item as={NavLink} to="/dictionary/categories">
          Categories
          <Counter loading={loadingQuantity} value={quantity?.countCategories} />
        </Menu.Item>
        <Menu.Item as={NavLink} to="/dictionary/rules">
          Rules
          <Counter loading={loadingQuantity} value={quantity?.countRules} />
        </Menu.Item>
      </Menu>
      <div className="mt-3">
        <Outlet />
      </div>
    </Container>
  );
});