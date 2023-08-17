import { Link } from 'react-router-dom';
import { Button, ButtonProps, Icon, IconProps } from 'semantic-ui-react';

interface Props {
  link: string;
  text?: React.ReactNode;
  btnProps?: ButtonProps;
  iconProps?: IconProps;
}

export default function LinkBack(
  {
    link,
    text = 'Back',
    btnProps = { secondary: true },
    iconProps = { name: 'angle left' }
  }: Props
) {
  return (
    <Button
      as={Link} to={link}
      {...btnProps}
    >
      <Icon {...iconProps} />
      {text}
    </Button>
  );
}