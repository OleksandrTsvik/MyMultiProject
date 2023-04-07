import { observer } from 'mobx-react-lite';
import { NavLink } from 'react-router-dom';
import { Container, Icon, Menu } from 'semantic-ui-react';

import { useStore } from '../stores/store';
import IconPill from '../../components/IconPill';

export default observer(function NavBar() {
    const { dutyStore } = useStore();
    const { countNotCompleted } = dutyStore;

    return (
        <Menu inverted fixed="top" icon="labeled">
            <Container>
                <Menu.Item
                    as={NavLink}
                    to="/"
                    header
                    style={{ fontWeight: 700 }}
                >
                    <Icon name="flask" />
                    MyMultiProject
                </Menu.Item>
                <Menu.Item as={NavLink} to="/tasks">
                    <IconPill
                        name="tasks"
                        color="teal"
                        value={countNotCompleted}
                    />
                    Tasks
                </Menu.Item>
                <Menu.Item
                    as={NavLink}
                    to="/test"
                    style={{ justifyContent: 'center' }}
                    name="404"
                />
            </Container>
        </Menu>
    );
});