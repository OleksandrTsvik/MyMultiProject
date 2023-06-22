import { makeAutoObservable, runInAction } from 'mobx';

import { User, UserLogin, UserRegister } from '../models/user';
import { store } from './store';
import agent from '../api/agent';
import { router } from '../router/Routes';

export default class UserStore {
    user: User | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    get isLoggedIn(): boolean {
        return !!this.user;
    }

    login = async (credentials: UserLogin) => {
        try {
            const user = await agent.Account.login(credentials);

            store.commonStore.setToken(user.token);
            await store.dutyStore.loadDuties();

            runInAction(() => {
                user.registrationDate = new Date(user.registrationDate);
                this.user = user;
            });

            router.navigate('/');
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    }

    register = async (credentials: UserRegister) => {
        try {
            const user = await agent.Account.register(credentials);

            store.commonStore.setToken(user.token);

            runInAction(() => {
                user.registrationDate = new Date(user.registrationDate);
                this.user = user;
            });

            router.navigate('/');
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    }

    logout = () => {
        store.commonStore.setToken(null);
        store.dutyStore.resetDutyStore();

        this.user = null;

        router.navigate('/');
    }

    getUser = async () => {
        try {
            const user = await agent.Account.current();

            await store.dutyStore.loadDuties();

            runInAction(() => {
                this.user = user;
            });
        } catch (error) {
            console.log(error);
        }
    }
}