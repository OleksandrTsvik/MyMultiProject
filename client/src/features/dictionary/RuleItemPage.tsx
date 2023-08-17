import { SyntheticEvent } from 'react';
import { observer } from 'mobx-react-lite';
import { Link, useParams } from 'react-router-dom';
import { Button, Container, Flag, Header, Label, Segment } from 'semantic-ui-react';

import { useStore } from '../../app/stores/store';
import ModalConfirm from '../../app/common/modals/ModalConfirm';
import LinkBack from '../../components/LinkBack';

export default observer(function RuleItemPage() {
  const { ruleId } = useParams();

  const { modalStore } = useStore();
  const { openModal } = modalStore;

  function handleOpenDeleteRule(event: SyntheticEvent) {
    event.stopPropagation();

    openModal(
      <ModalConfirm
        content={`Delete the rule ${'title'}.`}
        onConfirm={() => console.log('onConfirm Rule')}
      />,
      {},
      true
    );
  }

  return (
    <Container>
      <LinkBack link="/dictionary/rules" />
      <Header as="h2" className="text-center">
        <Label horizontal color="teal">status</Label>
        <Flag name="america" className="mx-2" />
        <div className="d-inline-block">Title --- ID {ruleId}</div>
      </Header>
      <Segment>
        Description
      </Segment>
      <div className="text-end">
        <Button
          content="Edit"
          color="blue"
          as={Link} to={`/dictionary/rules/edit/${ruleId}`}
        />
        <Button
          content="Delete"
          color="red"
          onClick={handleOpenDeleteRule}
        />
      </div>
    </Container>
  );
});