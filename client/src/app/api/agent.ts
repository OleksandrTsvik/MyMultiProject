import axios, { AxiosError, AxiosHeaders, AxiosRequestConfig, AxiosResponse } from 'axios';
import { toast } from 'react-toastify';

import { store } from '../stores/store';
import { router } from '../router/Routes';
import { PaginatedResult } from '../models/pagination';
import sleep from '../utils/sleep';
import { Duties } from './duties';
import { Account } from './account';
import { DictionaryCategories, DictionaryItems } from './dictionary';
import { Profiles } from './profiles';

export const baseUrl = process.env.REACT_APP_API_URL;

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
    if (process.env.NODE_ENV === 'development') {
      await sleep(1000);
    }

    const pagination = response.headers['pagination'];

    if (pagination) {
      response.data = new PaginatedResult(response.data, JSON.parse(pagination));

      return response as AxiosResponse<PaginatedResult<any>>;
    }

    return response;
  },
  (error: AxiosError) => {
    const { data, status, config, headers } = error.response as AxiosResponse;
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
        if (headers['www-authenticate']?.startsWith('Bearer error="invalid_token"')) {
          store.userStore.logout();
          toast.error('Session expired - please login again');
        } else {
          toast.error('Unauthorised');
        }

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

export const responseBody = <T>(response: AxiosResponse<T>) => response.data;

export const requests = {
  get: <T>(url: string, config?: AxiosRequestConfig<any>) => axios.get<T>(url, config).then(responseBody),
  post: <T>(url: string, body?: {}) => axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
  patch: <T>(url: string, body: {}) => axios.patch<T>(url, body).then(responseBody)
};

export const uploadFile = <T>(file: Blob, url: string, fieldName: string = 'File') => {
  let formData = new FormData();
  formData.append(fieldName, file);

  return axios
    .post<T>(url, formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })
    .then(responseBody);
}

const agent = {
  Duties,
  Account,
  Profiles,
  DictionaryCategories,
  DictionaryItems
};

export default agent;