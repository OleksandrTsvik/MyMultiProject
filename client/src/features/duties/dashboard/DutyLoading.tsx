import { Button, ButtonProps, Card, Grid, Placeholder } from 'semantic-ui-react';

interface Props {
    countCards?: number;
}

const buttons: ButtonProps[] = [
    { inverted: false, icon: 'tint', color: 'purple' },
    { inverted: true, icon: 'trash alternate', color: 'red' },
    { inverted: true, icon: 'pencil alternate', color: 'blue' },
    { inverted: true, icon: 'check', color: 'green' }
];

export default function DutyLoading({ countCards = 4 }: Props) {
    return (
        <Grid className="py-2">
            {Array(countCards).fill(null).map((_, i) => (
                <Grid.Column
                    key={i}
                    mobile={16}
                    tablet={8}
                    computer={4}
                >
                    <Card fluid>
                        <Card.Content>
                            <Card.Header>
                                <Placeholder fluid>
                                    <Placeholder.Header>
                                        <Placeholder.Line />
                                        <Placeholder.Line />
                                    </Placeholder.Header>
                                </Placeholder>
                            </Card.Header>
                        </Card.Content>
                        <Card.Content>
                            <Placeholder fluid>
                                <Placeholder.Paragraph>
                                    <Placeholder.Line />
                                    <Placeholder.Line />
                                    <Placeholder.Line />
                                </Placeholder.Paragraph>
                            </Placeholder>
                        </Card.Content>
                        <Card.Content>
                            <Placeholder fluid>
                                <Placeholder.Header image>
                                    <Placeholder.Line />
                                    <Placeholder.Line />
                                </Placeholder.Header>
                            </Placeholder>
                        </Card.Content>
                        <Card.Content className="d-flex flex-row-reverse gap-2">
                            {buttons.map((btn, index) => (
                                <Button
                                    key={index}
                                    disabled
                                    circular
                                    className="m-0"
                                    {...btn}
                                />
                            ))}
                        </Card.Content>
                    </Card>
                </Grid.Column>
            ))}
        </Grid>
    );
}