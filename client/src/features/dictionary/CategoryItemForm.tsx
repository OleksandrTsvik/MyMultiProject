import { Button, Container, Form, Header, TextArea } from 'semantic-ui-react';

import LinkBack from '../../components/LinkBack';
import StatusDropdown from './StatusDropdown';

interface Props {
  title: string;
  textForSubmitBtn: string;
  linkToBack: string;
  textToBack?: string;
}

export default function CategoryItemForm(
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
          control={StatusDropdown}
          label='Status'
        />
        <Form.Field
          control={TextArea}
          label='Text'
        />
        <Form.Field
          control={TextArea}
          label='Translation'
        />
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