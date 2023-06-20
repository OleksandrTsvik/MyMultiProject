import { createContext, useContext } from 'react';

import DutyStore from './dutyStore';
import CommonStore from './commonStore';
import UserStore from './userStore';
import ModalStore from './modalStore';

interface Store {
    dutyStore: DutyStore;
    commonStore: CommonStore;
    userStore: UserStore;
    modalStore: ModalStore;
}

export const store: Store = {
    dutyStore: new DutyStore(),
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore()
};

export const StoreContext = createContext(store);

export function useStore(): Store {
    return useContext(StoreContext);
}