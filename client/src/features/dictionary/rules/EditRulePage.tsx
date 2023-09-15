import { useEffect, useState } from 'react';
import { Navigate, useParams } from 'react-router-dom';

import { GrammarRule } from '../../../app/models/dictionary';
import agent from '../../../app/api/agent';
import Loading from '../../../components/Loading';
import RuleForm, { GrammarRuleForm } from './RuleForm';

export default function EditRulePage() {
  const { ruleId } = useParams();

  const [grammarRule, setGrammarRule] = useState<GrammarRule>();
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    if (ruleId) {
      agent.GrammarRules.details(ruleId)
        .then((data) => setGrammarRule(data))
        .finally(() => setLoading(false));
    }
  }, [ruleId]);

  async function handleSubmit(editedRule: GrammarRuleForm) {
    if (!grammarRule) {
      return;
    }

    await agent.GrammarRules.update({
      ...editedRule,
      id: grammarRule.id
    });
  }

  if (loading) {
    return <Loading content="Loading grammar rule..." />;
  }

  if (!grammarRule) {
    return <Navigate to="/not-found" replace />;
  }

  return (
    <>
      <RuleForm
        title="Update rule"
        textForSubmitBtn="Update"
        linkToBack={`/dictionary/rules/${ruleId}`}
        initialValues={grammarRule}
        onSubmit={handleSubmit}
      />
    </>
  );
}