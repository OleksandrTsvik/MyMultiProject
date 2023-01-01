import { Button, Icon, Label } from "semantic-ui-react";

interface Props {
    openCreateMode: () => void;
    countNotCompletedDuties: number;
}

export default function DutyCreate({ countNotCompletedDuties, openCreateMode }: Props) {
    return (
        <div className='mb-3 text-end'>
            <Button as='div' labelPosition='right'
                onClick={openCreateMode}
            >
                <Button positive>
                    <Icon name='add' />
                    Create
                </Button>
                <Label basic color='green' pointing='left'>
                    {countNotCompletedDuties}
                </Label>
            </Button>
        </div>
    );
}