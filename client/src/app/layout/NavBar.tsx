import React from 'react';
import { Container, Icon, Menu } from 'semantic-ui-react';
import { useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';
import IconPill from '../../features/components/IconPill';

export default observer(function NavBar() {
    const { dutyStore } = useStore();
    const { countNotCompleted } = dutyStore;

    return (
        <Menu inverted fixed='top' icon='labeled'>
            <Container>
                <Menu.Item header>
                    <Icon name='flask' />
                    MyMultiProject
                </Menu.Item>
                <Menu.Item>
                    <IconPill
                        name='tasks'
                        color='teal'
                        value={countNotCompleted}
                    />
                    Tasks
                </Menu.Item>
                <Menu.Item name='Test' style={{ justifyContent: 'center' }} />
            </Container>
        </Menu>
    );
});