import { CSSProperties, useCallback, useMemo } from 'react';
import { Header, Icon } from 'semantic-ui-react';
import { useDropzone } from 'react-dropzone';

import { DropzoneFile } from './PhotoUploadWidget';

interface Props {
  setFiles: (files: DropzoneFile[]) => void;
}

const dropzoneStyles: CSSProperties = {
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  justifyContent: 'center',
  minHeight: 250,
  padding: 20,
  backgroundColor: '#fafafa',
  border: 'dashed 3px #eeeeee',
  borderColor: '#eeeeee',
  borderRadius: 5,
  outline: 'none',
  transition: 'border .24s ease-in-out'
};

const dropzoneActive: CSSProperties = {
  borderColor: '#00b5ad'
};

export default function PhotoWidgetDropzone({ setFiles }: Props) {
  const onDrop = useCallback((acceptedFiles: File[]) => {
    setFiles(acceptedFiles.map((file: File) => (
      Object.assign(file, {
        preview: URL.createObjectURL(file)
      })
    )));
  }, [setFiles]);

  const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop });

  const style = useMemo(() => ({
    ...dropzoneStyles,
    ...(isDragActive ? dropzoneActive : {})
  }), [isDragActive]);

  return (
    <div {...getRootProps({ style })}>
      <input {...getInputProps()} />
      <Icon name="upload" size="huge" />
      <Header
        content={isDragActive
          ? 'Drop the files here ...'
          : 'Drag \'n\' drop some files here, or click to select files'
        }
      />
    </div>
  );
}