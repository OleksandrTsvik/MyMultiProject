import { observer } from 'mobx-react-lite';
import { NavLink } from 'react-router-dom';
import { Container, Dropdown, Icon, Menu } from 'semantic-ui-react';

import { useStore } from '../stores/store';
import IconPill from '../../components/IconPill';

export default observer(function NavBar() {
    const { dutyStore } = useStore();
    const { countNotCompleted } = dutyStore;

    return (
        <Menu
            inverted
            fixed="top"
            icon="labeled"
            size="tiny"
        >
            <Container>
                <Menu.Item
                    as={NavLink} to="/"
                    header
                    className="fw-bold justify-content-center"
                >
                    <Icon name="flask" />
                    MyMultiProject
                </Menu.Item>
                <Menu.Item
                    as={NavLink} to="/tasks"
                    className="justify-content-center"
                >
                    <IconPill
                        name="tasks"
                        color="teal"
                        value={countNotCompleted}
                    />
                    Tasks
                </Menu.Item>
                <Dropdown
                    item
                    icon={
                        <>
                            <Icon name="language" />
                            Translate
                            <Icon name="dropdown" />
                        </>
                    }
                    className="justify-content-center"
                >
                    <Dropdown.Menu>
                        <Dropdown.Item
                            as={NavLink} to="/translate/language"
                            icon="language"
                            text="Language"
                        />
                        <Dropdown.Item
                            as={NavLink} to="/translate/keyboard"
                            icon="keyboard"
                            text="Keyboard"
                        />
                    </Dropdown.Menu>
                </Dropdown>
                <Menu.Item
                    as={NavLink} to="/games"
                    className="justify-content-center"
                >
                    <Icon name="gamepad" />
                    Games
                </Menu.Item>
                <Menu.Item
                    as={NavLink} to="/errors"
                    className="justify-content-center"
                >
                    <Icon name="bug" />
                    Errors
                </Menu.Item>
                <Menu.Item
                    as={NavLink} to="/not-found"
                    name="404"
                    className="justify-content-center"
                />
            </Container>
        </Menu>
    );
});