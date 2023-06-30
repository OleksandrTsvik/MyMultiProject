import { createContext, useContext } from 'react';

import DutyStore from './dutyStore';
import CommonStore from './commonStore';
import UserStore from './userStore';
import ModalStore from './modalStore';
import ProfileStore from './profileStore';

interface Store {
    dutyStore: DutyStore;
    commonStore: CommonStore;
    userStore: UserStore;
    modalStore: ModalStore;
    profileStore: ProfileStore;
}

export const store: Store = {
    dutyStore: new DutyStore(),
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    profileStore: new ProfileStore()
};

export const StoreContext = createContext(store);

export function useStore(): Store {
    return useContext(StoreContext);
}