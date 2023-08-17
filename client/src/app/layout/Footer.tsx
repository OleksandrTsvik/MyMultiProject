import { Container } from 'semantic-ui-react';

export default function Footer() {
  return (
    <footer className="footer">
      <Container textAlign="center">
        © 2022 - {(new Date()).getFullYear()}. Copyright:&ensp;
        <a href="mailto:oleksandr.zwick@gmail.com">oleksandr.zwick@gmail.com</a>
      </Container>
    </footer>
  );
}