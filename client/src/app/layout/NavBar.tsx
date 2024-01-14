import { observer } from 'mobx-react-lite';
import { NavLink } from 'react-router-dom';
import { Container, Dropdown, Icon, Image, Menu, Popup, Sticky } from 'semantic-ui-react';

import { useStore } from '../stores/store';
import IconPill from '../../components/IconPill';
import LoginForm from '../../features/users/LoginForm';
import RegisterForm from '../../features/users/RegisterForm';

export default observer(function NavBar() {
  const { userStore, modalStore } = useStore();

  const { isLoggedIn, user, logout, countNotCompletedDuties } = userStore;
  const { openModal } = modalStore;

  return (
    <Sticky>
      <Menu inverted icon="labeled" size="tiny" className="menu__nav">
        <Container className="flex-wrap">
          <Menu.Item
            as={NavLink}
            to="/"
            header
            className="fw-bold justify-content-center"
          >
            <Icon name="flask" />
            MyMultiProject
          </Menu.Item>
          <Menu.Item
            as={NavLink}
            to="/tasks"
            className="justify-content-center"
          >
            <IconPill
              name="tasks"
              color="teal"
              value={countNotCompletedDuties}
            />
            Tasks
          </Menu.Item>
          <Menu.Item
            as={NavLink}
            to="/dictionary"
            className="justify-content-center"
          >
            <Icon name="book" />
            Dictionary
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
                as={NavLink}
                to="/translate/language"
                icon="language"
                text="Language"
              />
              <Dropdown.Item
                as={NavLink}
                to="/translate/keyboard"
                icon="keyboard"
                text="Keyboard"
              />
            </Dropdown.Menu>
          </Dropdown>
          <Menu.Item
            as={NavLink}
            to="/games"
            className="justify-content-center"
          >
            <Icon name="gamepad" />
            Games
          </Menu.Item>
          <Menu.Item
            as={NavLink}
            to="/birthdays"
            className="justify-content-center"
          >
            <Icon name="gift" />
            Birthdays
          </Menu.Item>
          <Menu.Item
            as={NavLink}
            to="/errors"
            className="justify-content-center"
          >
            <Icon name="bug" />
            Errors
          </Menu.Item>
          <Menu.Item
            as={NavLink}
            to="/not-found"
            name="404"
            className="justify-content-center"
          />
          <Menu.Menu position="right">
            {isLoggedIn ? (
              <Dropdown
                item
                icon={
                  <>
                    {/* <Icon name="user circle" /> */}
                    <Image
                      avatar
                      className="me-0 mb-2"
                      src={user?.image || '/assets/user.png'}
                    />
                    <Popup
                      content={
                        <span className="text-break">{user?.userName}</span>
                      }
                      position="bottom right"
                      trigger={
                        <span className="truncate-text menu__username">
                          {user?.userName}
                        </span>
                      }
                    />
                    <Icon name="dropdown" />
                  </>
                }
                className="justify-content-center"
              >
                <Dropdown.Menu>
                  <Dropdown.Item
                    as={NavLink}
                    to={`/profiles/${user?.userName}`}
                    icon="user outline"
                    text="My Profile"
                  />
                  <Dropdown.Item onClick={logout} icon="power" text="Logout" />
                </Dropdown.Menu>
              </Dropdown>
            ) : (
              <>
                <Menu.Item
                  onClick={() => openModal(<LoginForm />, { size: 'mini' })}
                  className="justify-content-center"
                >
                  <Icon name="sign in" />
                  Login
                </Menu.Item>
                <Menu.Item
                  onClick={() => openModal(<RegisterForm />, { size: 'mini' })}
                  className="justify-content-center"
                >
                  <Icon name="user plus" />
                  Register
                </Menu.Item>
              </>
            )}
          </Menu.Menu>
        </Container>
      </Menu>
    </Sticky>
  );
});