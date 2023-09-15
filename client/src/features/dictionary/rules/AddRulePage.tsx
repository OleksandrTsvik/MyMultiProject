import agent from '../../../app/api/agent';
import RuleForm, { GrammarRuleForm } from './RuleForm';

export default function AddRulePage() {
  async function handleSubmit(createdRule: GrammarRuleForm) {
    await agent.GrammarRules.create(createdRule);
  }

  return (
    <>
      <RuleForm
        title="Add a new rule"
        textForSubmitBtn="Add"
        linkToBack="/dictionary/rules"
        initialValues={{
          title: '',
          description: '',
          language: '',
        }}
        onSubmit={handleSubmit}
      />
    </>
  );
}