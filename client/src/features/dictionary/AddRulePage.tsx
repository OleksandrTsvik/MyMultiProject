import RuleForm from './RuleForm';

export default function AddRulePage() {
    return (
        <>
            <RuleForm
                title="Add a new rule"
                textForSubmitBtn="Add"
                linkToBack="/dictionary/rules"
            />
        </>
    );
}