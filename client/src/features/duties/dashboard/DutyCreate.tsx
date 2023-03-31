import { observer } from 'mobx-react-lite';
import { Button, Icon, Label } from 'semantic-ui-react';

import { useStore } from '../../../app/stores/store';

export default observer(function DutyCreate() {
    const { dutyStore } = useStore();
    const { countNotCompleted, openCreateMode } = dutyStore;

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
                    {countNotCompleted}
                </Label>
            </Button>
        </div>
    );
});