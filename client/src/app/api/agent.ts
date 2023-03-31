import axios, { AxiosResponse } from 'axios';

import { Duty } from '../models/duty';
import sleep from '../utils/sleep';

axios.defaults.baseURL = 'http://localhost:5000/api';

axios.interceptors.response.use(async (response) => {
    try {
        await sleep(1000);
        return response;
    } catch (error) {
        console.log(error);
        return await Promise.reject(error);
    }
});

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody)
};

const Duties = {
    list: () => requests.get<Duty[]>('/duties'),
    details: (id: string) => requests.get<Duty>(`/duties/${id}`),
    create: (duty: Duty) => axios.post<void>('/duties', duty),
    update: (duty: Duty) => axios.put<void>(`/duties/${duty.id}`, duty),
    delete: (id: string) => axios.delete<void>(`/duties/${id}`)
};

const agent = {
    Duties
};

export default agent;