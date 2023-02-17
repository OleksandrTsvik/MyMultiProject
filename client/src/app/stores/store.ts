import { createContext, useContext } from 'react';
import DutyStore from './dutyStore';

interface Store {
    dutyStore: DutyStore;
}

export const store: Store = {
    dutyStore: new DutyStore()
};

export const StoreContext = createContext(store);

export function useStore(): Store {
    return useContext(StoreContext);
}