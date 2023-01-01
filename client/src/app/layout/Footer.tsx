import React from 'react';
import { Container } from 'semantic-ui-react';

export default function Footer() {
    return (
        <footer>
            <Container textAlign='center'>
                © 2022 - {(new Date()).getFullYear()}. Copyright:&ensp;
                <a href="mailto:ipz203_tsos@student.ztu.edu.ua">ipz203_tsos@student.ztu.edu.ua</a>
            </Container>
        </footer>
    );
}