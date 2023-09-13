import { CSSProperties, useState } from 'react';
import { useField } from 'formik';
import { Form, Label } from 'semantic-ui-react';
import { ContentState, EditorState, convertToRaw } from 'draft-js';
import draftToHtml from 'draftjs-to-html';
import htmlToDraft from 'html-to-draftjs';

import { TextEditorProps } from '../../../components/TextEditor';
import TextEditorWithImages from '../imageUpload/TextEditorWithImages';

interface Props extends TextEditorProps {
  name: string;
  label?: string;
  labelStyle?: CSSProperties;
}

export default function FormikTextEditor(
  {
    name,
    label,
    labelStyle,
    ...props
  }: Props
) {
  const [field, meta, helpers] = useField(name);

  const [editorState, setEditorState] = useState<EditorState>(
    field.value ? prepareDraft(field.value) : EditorState.createEmpty()
  );

  return (
    <Form.Field error={meta.touched && !!meta.error}>
      {label && <label style={labelStyle}>{label}</label>}
      <TextEditorWithImages
        {...props}
        editorState={editorState}
        onEditorStateChange={(editorState) => {
          setEditorState(editorState);

          helpers.setValue(draftToHtml(
            convertToRaw(editorState.getCurrentContent())
          ));
        }}
        onBlur={() => helpers.setTouched(true)}
      // error={meta.touched && !!meta.error}
      />
      {meta.touched && meta.error && <Label basic pointing color="red" content={meta.error} />}
    </Form.Field>
  );
}

export function prepareDraft(value: string) {
  const draft = htmlToDraft(value);

  const contentState = ContentState.createFromBlockArray(draft.contentBlocks);
  const editorState = EditorState.createWithContent(contentState);

  return editorState;
};