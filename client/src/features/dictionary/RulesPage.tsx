import { Link } from 'react-router-dom';
import { Button, Flag, Icon, Label, List } from 'semantic-ui-react';

export default function RulesPage() {
    return (
        <>
            <div className="text-end">
                <Button>
                    <Icon name="add" />
                    Add rule
                </Button>
            </div>
            <List celled size="big">
                {Array(15).fill(null).map((_, index) => (
                    <List.Item
                        key={index}
                        as={Link} to={`/dictionary/rules/${index}`}
                        className="grammar-rule__item d-flex align-items-center"
                    >
                        <div>
                            <Icon
                                className="me-3"
                                name="block layout"
                                size="small"
                            />
                            <Flag name="america" />
                            <Label horizontal color="teal">Status</Label>
                            Rule
                        </div>
                    </List.Item>
                ))}
            </List>
        </>
    );
}