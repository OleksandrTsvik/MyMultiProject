import { useEffect, useState } from 'react';
import { Button, Grid, Header } from 'semantic-ui-react';

import PhotoWidgetDropzone from './PhotoWidgetDropzone';
import PhotoWidgetCropper from './PhotoWidgetCropper';

export interface DropzoneFile extends File {
  preview: string;
}

interface Props {
  loading: boolean;
  onPhotoUpload: (file: Blob) => void;
}

export default function PhotoUploadWidget({ loading, onPhotoUpload }: Props) {
  const [files, setFiles] = useState<DropzoneFile[]>([]);
  const [cropper, setCropper] = useState<Cropper>();

  function handleCrop() {
    if (cropper) {
      cropper.getCroppedCanvas()
        .toBlob((blob) => {
          if (blob) {
            onPhotoUpload(blob);
          }
        });
    }
  }

  useEffect(() => {
    return () => {
      files.forEach((file) => URL.revokeObjectURL(file.preview));
    };
  }, [files]);

  return (
    <Grid>
      <Grid.Column width={4}>
        <Header
          sub
          color="teal"
          content="Step 1 - Add Photo"
        />
        <PhotoWidgetDropzone setFiles={setFiles} />
      </Grid.Column>
      <Grid.Column width={1} />
      <Grid.Column width={4}>
        <Header
          sub
          color="teal"
          content="Step 2 - Resize Image"
        />
        {files.length > 0 &&
          <PhotoWidgetCropper
            setCropper={setCropper}
            imagePreview={files[0].preview}
          />
        }
      </Grid.Column>
      <Grid.Column width={1} />
      <Grid.Column width={4}>
        <Header
          sub
          color="teal"
          content="Step 3 - Preview & Upload"
        />
        {files.length > 0 &&
          <>
            <div
              className="cropper__img-preview"
              style={{ minHeight: 250, overflow: 'hidden' }}
            />
            <Button.Group fluid>
              <Button
                positive
                loading={loading}
                disabled={loading}
                icon="check"
                onClick={handleCrop}
              />
              <Button
                disabled={loading}
                color="red"
                icon="close"
                onClick={() => setFiles([])}
              />
            </Button.Group>
          </>
        }
      </Grid.Column>
    </Grid>
  );
}