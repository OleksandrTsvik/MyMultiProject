import { Label, LabelProps, Loader, LoaderProps } from 'semantic-ui-react';

interface Props extends LabelProps {
  value?: number;
  loading?: boolean;
  loaderProps?: LoaderProps;
}

export default function Counter(
  {
    value,
    loading,
    loaderProps,
    color = 'blue',
    ...props
  }: Props
) {
  return (
    <>
      {loading
        ? <Loader
          active
          size="mini"
          className="position-relative"
          style={{ marginLeft: 12, top: 0, left: 0, transform: 'none' }}
          {...loaderProps}
        />
        : <Label color={color} {...props}>{value}</Label>
      }
    </>
  );
}