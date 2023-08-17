import { makeAutoObservable, runInAction } from 'mobx';

import { User, UserLogin, UserRegister } from '../models/user';
import { router } from '../router/Routes';
import agent from '../api/agent';
import { store } from './store';

export default class UserStore {
  user: User | null = null;
  refreshTokenTimeout?: ReturnType<typeof setTimeout>;

  constructor() {
    makeAutoObservable(this);
  }

  get countNotCompletedDuties(): number {
    return this.user ? this.user.countNotCompletedDuties : 0;
  }

  get countCompletedDuties(): number {
    return this.user ? this.user.countCompletedDuties : 0;
  }

  get isLoggedIn(): boolean {
    return !!this.user;
  }

  login = async (credentials: UserLogin) => {
    try {
      const user = await agent.Account.login(credentials);

      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);

      await store.dutyStore.loadDuties();

      runInAction(() => {
        user.registrationDate = new Date(user.registrationDate);
        this.user = user;
      });

      router.navigate('/');
      store.modalStore.closeModal();
    } catch (error) {
      console.log(error);

      throw error;
    }
  }

  register = async (credentials: UserRegister) => {
    try {
      const user = await agent.Account.register(credentials);

      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);

      runInAction(() => {
        user.registrationDate = new Date(user.registrationDate);
        this.user = user;
      });

      router.navigate('/');
      store.modalStore.closeModal();
    } catch (error) {
      console.log(error);

      throw error;
    }
  }

  logout = () => {
    this.stopRefreshTokenTimer();

    store.commonStore.setToken(null);
    store.dutyStore.resetDutyStore();

    this.user = null;

    router.navigate('/');
  }

  getUser = async () => {
    try {
      const user = await agent.Account.current();

      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);

      runInAction(() => {
        this.user = user;
      });
    } catch (error) {
      console.log(error);
    }
  }

  refreshToken = async () => {
    this.stopRefreshTokenTimer();

    try {
      const user = await agent.Account.refreshToken();

      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);

      runInAction(() => {
        this.user = user;
      });
    } catch (error) {
      console.log(error);
    }
  }

  private startRefreshTokenTimer(user: User) {
    const jwt = JSON.parse(atob(user.token.split('.')[1]));

    // const bufferJWT = Buffer.from(user.token.split('.')[1], 'base64');
    // const jwt = JSON.parse(bufferJWT.toString());

    const expires = new Date(jwt.exp * 1000);
    const timeout = expires.getTime() - Date.now() - (60 * 1000);

    this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }

  setImage = (image: string | undefined) => {
    if (this.user) {
      this.user.image = image;
    }
  }

  updateCountCompletedDuties = (num: number) => {
    if (!this.user) {
      return;
    }

    this.user.countCompletedDuties += num;
    this.user.countNotCompletedDuties -= num;
  }

  updateCountDuties = (num: number, isCompleted: boolean) => {
    if (!this.user) {
      return;
    }

    if (isCompleted) {
      this.user.countCompletedDuties += num;
    } else {
      this.user.countNotCompletedDuties += num;
    }
  }
}