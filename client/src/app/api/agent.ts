import axios, { AxiosError, AxiosHeaders, AxiosResponse } from 'axios';
import { toast } from 'react-toastify';

import { store } from '../stores/store';
import { router } from '../router/Routes';
import { Duty } from '../models/duty';
import { User, UserLogin, UserRegister } from '../models/user';
import { ListFollowingsPredicate, Photo, Profile } from '../models/profile';
import sleep from '../utils/sleep';

export const baseUrl = 'http://localhost:5000/api';

axios.defaults.baseURL = baseUrl;

axios.interceptors.request.use(
    (config) => {
        const token = store.commonStore.token;

        if (token && config.headers) {
            (config.headers as AxiosHeaders)
                .set('Authorization', `Bearer ${token}`);
        }

        return config;
    }
);

axios.interceptors.response.use(
    async (response) => {
        await sleep(1000);
        return response;
    },
    (error: AxiosError) => {
        const { data, status, config } = error.response as AxiosResponse;
        const { errors } = data;

        switch (status) {
            case 400:
                if (errors && config.method === 'get' && errors.hasOwnProperty('id')) {
                    router.navigate('not-found');
                } else if (errors) {
                    const arrayErrors = [];

                    for (const key in errors) {
                        if (errors[key]) {
                            arrayErrors.push(errors[key]);
                        }
                    }

                    throw arrayErrors.flat();
                } else {
                    toast.error(data);
                    throw data;
                }

                break;
            case 401:
                toast.error('Unauthorised');
                break;
            case 403:
                toast.error('Forbidden');
                break;
            case 404:
                router.navigate('not-found');
                break;
            case 500:
                store.commonStore.setServerError(data);
                router.navigate('server-error');
                break;
            default:
                break;
        }

        return Promise.reject(error);
    }
);

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
    create: (duty: Duty) => requests.post<void>('/duties', duty),
    update: (duty: Duty) => requests.put<void>(`/duties/${duty.id}`, duty),
    delete: (id: string) => requests.delete<void>(`/duties/${id}`),
    updateList: (duties: Duty[]) => requests.put<void>('/duties/list', duties)
};

const Account = {
    current: () => requests.get<User>('/account'),
    login: (user: UserLogin) => requests.post<User>('/account/login', user),
    register: (user: UserRegister) => requests.post<User>('/account/register', user)
};

const Profiles = {
    get: (username: string) => requests.get<Profile>(`/profiles/${username}`),
    uploadPhoto: (file: Blob) => {
        let formData = new FormData();
        formData.append('File', file);

        return axios
            .post<Photo>('/photos', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
            .then(responseBody);
    },
    setMainPhoto: (id: string) => requests.post<void>(`/photos/${id}/setMain`, {}),
    deletePhoto: (id: string) => requests.delete<void>(`/photos/${id}`),
    updateFollowing: (username: string) => requests.post<void>(`/follow/${username}`, {}),
    listFollowings: (username: string, predicate: ListFollowingsPredicate) =>
        requests.get<Profile[]>(`/follow/${username}?predicate=${predicate}`)
};

const agent = {
    Duties,
    Account,
    Profiles
};

export default agent;