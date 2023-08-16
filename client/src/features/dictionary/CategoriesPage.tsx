import { Link } from 'react-router-dom';
import { Button, Flag, Icon, Label, List } from 'semantic-ui-react';

export default function CategoriesPage() {
    return (
        <>
            <div className="text-end">
                <Button>
                    <Icon name="add" />
                    Add category
                </Button>
            </div>
            <List celled size="big">
                {Array(15).fill(null).map((_, index) => (
                    <List.Item
                        key={index}
                        as={Link} to={`/dictionary/categories/${index}`}
                        className="dictionary__categories-item"
                    >
                        <div className="d-flex align-items-center justify-content-between">
                            <div>
                                <Icon
                                    className="me-3"
                                    name="block layout"
                                    size="small"
                                />
                                <Flag name="america" />
                                Category
                            </div>
                            <Label color="teal">11</Label>
                        </div>
                    </List.Item>
                ))}
            </List>
        </>
    );
}