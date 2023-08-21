import { Editor, EditorProps } from 'react-draft-wysiwyg';

export interface EditorUploadImage {
  data: { link: string };
}

export interface TextEditorProps extends EditorProps {
  onUploadImage?: (file: File) => Promise<EditorUploadImage>;
}

export default function TextEditor({ onUploadImage, toolbar, ...props }: TextEditorProps) {
  return (
    <Editor
      wrapperClassName="draft-wysiwyg__wrapper"
      editorClassName="draft-wysiwyg__editor"
      {...props}
      toolbar={{
        options: [
          'fontSize', 'fontFamily', 'blockType', 'inline',
          'textAlign', 'colorPicker', 'list',
          'link', 'image', 'emoji', 'remove', 'history' // 'embedded'
        ],
        image: {
          previewImage: true,
          alt: { present: true, mandatory: false },
          inputAccept: 'image/gif,image/jpeg,image/jpg,image/png,image/svg',
          defaultSize: {
            height: '100%',
            width: '100%'
          },
          uploadCallback: onUploadImage,
        },
        ...toolbar
      }}
    />
  );
}