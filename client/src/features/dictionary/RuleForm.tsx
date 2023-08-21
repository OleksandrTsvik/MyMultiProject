import { Button, Container, Form, Header, Input, TextArea } from 'semantic-ui-react';

import TextEditorWithImages from '../../app/common/imageUpload/TextEditorWithImages';
import LinkBack from '../../components/LinkBack';
import CountryDropdown from '../../components/CountryDropdown';
import StatusDropdown from './StatusDropdown';

interface Props {
  title: string;
  textForSubmitBtn: string;
  linkToBack: string;
  textToBack?: string;
}

export default function RuleForm(
  {
    title,
    textForSubmitBtn,
    linkToBack,
    textToBack = 'Back'
  }: Props
) {
  return (
    <Container>
      <div className="mb-3">
        <LinkBack link={linkToBack} text={textToBack} />
      </div>
      <Header as="h2" className="text-center">{title}</Header>
      <Form>
        <Form.Field
          control={CountryDropdown}
          label="Language"
        />
        <Form.Field
          control={StatusDropdown}
          label="Status"
        />
        <Form.Field
          control={Input}
          label="Title"
        />
        <Form.Field
          control={TextArea}
          label="Description"
        />
        <div className="mb-3">
          <TextEditorWithImages />
        </div>
        <Button
          type="submit"
          primary
          fluid
        >
          {textForSubmitBtn}
        </Button>
      </Form>
    </Container>
  );
}