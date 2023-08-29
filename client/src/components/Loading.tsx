import { Dimmer, Loader } from 'semantic-ui-react';

interface Props {
  inverted?: boolean;
  content?: string;
  dimmer?: boolean;
}

export default function Loading(
  {
    inverted = true,
    content = 'Loading...',
    dimmer = true
  }: Props
) {
  return (
    <>
      {dimmer
        ? <Dimmer active inverted={inverted}>
          <Loader content={content} />
        </Dimmer>
        : <Loader active content={content} />
      }
    </>
  );
}