import { Button, Form, Input, Modal } from 'semantic-ui-react';

import CountryDropdown from '../../components/CountryDropdown';

interface Props {
  title: string;
  textForSubmitBtn: string;
}

export default function CategoryModalForm(
  {
    title,
    textForSubmitBtn
  }: Props
) {
  return (
    <>
      <Modal.Header>{title}</Modal.Header>
      <Modal.Content>
        <Form>
          <Form.Field
            control={CountryDropdown}
            label="Language"
          />
          <Form.Field
            control={Input}
            label="Title"
          />
        </Form>
      </Modal.Content>
      <Modal.Actions>
        <Button
          type="submit"
          primary
        >
          {textForSubmitBtn}
        </Button>
      </Modal.Actions>
    </>
  );
}