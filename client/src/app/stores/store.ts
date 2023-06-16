import { createContext, useContext } from 'react';

import DutyStore from './dutyStore';
import CommonStore from './commonStore';

interface Store {
    dutyStore: DutyStore;
    commonStore: CommonStore;
}

export const store: Store = {
    dutyStore: new DutyStore(),
    commonStore: new CommonStore()
};

export const StoreContext = createContext(store);

export function useStore(): Store {
    return useContext(StoreContext);
}