import agent from '../../api/agent';
import TextEditor, { EditorUploadImage, TextEditorProps } from '../../../components/TextEditor';

export default function TextEditorWithImages(props: TextEditorProps) {
  async function handleUploadImage(file: File): Promise<EditorUploadImage> {
    try {
      const resultUpload = await agent.Profiles.uploadImage(file);

      return { data: { link: resultUpload.url } };
    } catch (error) {
      console.log(error);

      throw error;
    }
  }

  return (
    <TextEditor
      onUploadImage={handleUploadImage}
      {...props}
    />
  );
}