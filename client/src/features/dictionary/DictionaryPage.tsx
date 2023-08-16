import { NavLink, Outlet } from 'react-router-dom';
import { Container, Label, Menu } from 'semantic-ui-react';

export default function DictionaryPage() {
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
                    <Label color="blue">10</Label>
                </Menu.Item>
                <Menu.Item as={NavLink} to="/dictionary/rules">
                    Rules
                    <Label color="blue">50</Label>
                </Menu.Item>
            </Menu>
            <div className="mt-3">
                <Outlet />
            </div>
        </Container>
    );
}