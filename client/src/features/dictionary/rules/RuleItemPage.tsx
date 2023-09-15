import { useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Link, Navigate, useNavigate, useParams } from 'react-router-dom';
import { Button, Container, Header, Segment } from 'semantic-ui-react';

import { GrammarRule } from '../../../app/models/dictionary';
import { useStore } from '../../../app/stores/store';
import agent from '../../../app/api/agent';
import ModalConfirm from '../../../app/common/modals/ModalConfirm';
import LinkBack from '../../../components/LinkBack';
import Loading from '../../../components/Loading';
import CustomFlag from '../../../components/CustomFlag';
import StatusLabel from '../StatusLabel';

export default observer(function RuleItemPage() {
  const navigate = useNavigate();
  const { ruleId } = useParams();

  const { modalStore } = useStore();
  const { openModal } = modalStore;

  const [grammarRule, setGrammarRule] = useState<GrammarRule>();
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    if (ruleId) {
      agent.GrammarRules.details(ruleId)
        .then((data) => setGrammarRule(data))
        .finally(() => setLoading(false));
    }
  }, [ruleId]);

  function handleOpenDeleteRule() {
    if (!grammarRule) {
      return;
    }

    openModal(
      <ModalConfirm
        content={`Delete the rule "${grammarRule?.title}"?`}
        onConfirm={() => agent.GrammarRules.delete(grammarRule.id)
          .then(() => navigate('/dictionary/rules'))
        }
      />,
      {},
      true
    );
  }

  if (loading) {
    return <Loading content="Loading grammar rule..." />;
  }

  if (!grammarRule) {
    return <Navigate to="/not-found" replace />;
  }

  return (
    <Container>
      <LinkBack link="/dictionary/rules" />
      <Header as="h2" className="text-center">
        <StatusLabel status={grammarRule.status} />
        <CustomFlag className="flag__medium mx-2" strName={grammarRule.language} />
        <div className="d-inline-block">{grammarRule.title}</div>
      </Header>
      <Segment dangerouslySetInnerHTML={{ __html: grammarRule.description }} />
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