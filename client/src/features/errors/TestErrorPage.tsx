import { useState } from 'react';
import { Button, Container, Header, Segment } from 'semantic-ui-react';
import axios from 'axios';

import { baseUrl } from '../../app/api/agent';
import ValidationError from './ValidationError';

export default function TestErrorsPage() {
    const [errors, setErrors] = useState<string[] | null>(null);

    function handleNotFound() {
        axios.get(baseUrl + '/buggy/not-found')
            .catch((err) => {
                setErrors(null);
                console.log(err.response);
            });
    }

    function handleBadRequest() {
        axios.get(baseUrl + '/buggy/bad-request')
            .catch((err) => {
                setErrors(null);
                console.log(err.response);
            });
    }

    function handleValidationError() {
        axios.post(baseUrl + '/duties', {})
            .catch((err) => {
                setErrors(err);
                console.log(err);
            });
    }

    function handleServerError() {
        axios.get(baseUrl + '/buggy/server-error')
            .catch((err) => {
                setErrors(null);
                console.log(err.response);
            });
    }

    function handleUnauthorised() {
        axios.get(baseUrl + '/buggy/unauthorised')
            .catch((err) => {
                setErrors(null);
                console.log(err.response);
            });
    }

    function handleBadGuid() {
        axios.get(baseUrl + '/duties/notaguid')
            .catch((err) => {
                setErrors(null);
                console.log(err.response);
            });
    }

    return (
        <Container>
            <Header as="h1" content="Test Error component" />
            <Segment>
                <Button.Group fluid className="flex-wrap">
                    <Button onClick={handleNotFound} content="Not Found" basic primary />
                    <Button onClick={handleBadRequest} content="Bad Request" basic primary />
                    <Button onClick={handleValidationError} content="Validation Error" basic primary />
                    <Button onClick={handleServerError} content="Server Error" basic primary />
                    <Button onClick={handleUnauthorised} content="Unauthorised" basic primary />
                    <Button onClick={handleBadGuid} content="Bad Guid" basic primary />
                </Button.Group>
            </Segment>
            {errors && <ValidationError errors={errors} />}
        </Container>
    )
}