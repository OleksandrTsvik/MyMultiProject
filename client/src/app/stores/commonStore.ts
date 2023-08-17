import { makeAutoObservable, reaction } from 'mobx';

import { ServerError } from '../models/serverError';

export default class CommonStore {
  error: ServerError | null = null;

  token: string | null = localStorage.getItem('jwt');
  appLoaded: boolean = false;

  constructor() {
    makeAutoObservable(this);

    reaction(
      () => this.token,
      (token) => {
        if (token) {
          localStorage.setItem('jwt', token);
        } else {
          localStorage.removeItem('jwt');
        }
      }
    );
  }

  setServerError = (error: ServerError) => {
    this.error = error;
  }

  resetServerError = () => {
    this.error = null;
  }

  setToken = (token: string | null) => {
    this.token = token;
  }

  setAppLoaded = (appLoaded: boolean = true) => {
    this.appLoaded = appLoaded;
  }
}