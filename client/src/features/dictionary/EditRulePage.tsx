import { useParams } from 'react-router-dom';

import RuleForm from './RuleForm';

export default function EditRulePage() {
  const { ruleId } = useParams();

  return (
    <>
      <RuleForm
        title="Update rule"
        textForSubmitBtn="Update"
        linkToBack={`/dictionary/rules/${ruleId}`}
      />
    </>
  );
}