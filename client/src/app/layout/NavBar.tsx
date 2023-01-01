import React from 'react';
import { Container, Icon, Label, Menu } from 'semantic-ui-react';

interface Props {
    countNotCompletedDuties: number;
}

export default function NavBar({ countNotCompletedDuties }: Props) {
    return (
        <Menu inverted fixed='top' icon='labeled'>
            <Container>
                <Menu.Item header>
                    <Icon name='flask' />
                    MyMultiProject
                </Menu.Item>
                <Menu.Item>
                    <Icon className='position-relative' name='tasks'>
                        {countNotCompletedDuties > 0 &&
                            <Label color='teal' floating circular
                                style={{ top: '-0.8em', left: '130%' }}
                            >
                                {countNotCompletedDuties > 99
                                    ? '99+'
                                    : countNotCompletedDuties
                                }
                            </Label>
                        }
                    </Icon>
                    Tasks
                </Menu.Item>
                <Menu.Item name='Test' style={{ justifyContent: 'center' }} />
            </Container>
        </Menu>
    );
}