import React from 'react';
import { Container, Icon, Menu } from 'semantic-ui-react';

export default function NavBar() {
    return (
        <Menu inverted fixed='top' icon='labeled'>
            <Container>
                <Menu.Item header>
                    <Icon name='flask' />
                    MyMultiProject
                </Menu.Item>
                <Menu.Item>
                    <Icon name='tasks' />
                    Tasks
                </Menu.Item>
                <Menu.Item name='Test' style={{ justifyContent: 'center' }} />
            </Container>
        </Menu>
    );
}