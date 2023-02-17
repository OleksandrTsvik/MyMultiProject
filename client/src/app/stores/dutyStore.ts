import { makeAutoObservable } from 'mobx';
import agent from '../api/agent';
import { Duty } from '../models/duty';

export default class DutyStore {
    duties: Duty[] = [];
    selectedDuty: Duty | undefined = undefined;

    createMode: boolean = false;
    editMode: boolean = false;
    deleteMode: boolean = false;

    loading: boolean = false;
    loadingInitial: boolean = false;

    constructor() {
        makeAutoObservable(this);
    }

    get countCompleted(): number {
        return this.duties.filter(duty => duty.isCompleted).length;
    }

    get countNotCompleted(): number {
        return this.duties.filter(duty => !duty.isCompleted).length;
    }

    loadDuties = async () => {
        this.setLoadingInitial(true);

        try {
            this.duties = await agent.Duties.list();
        } catch (error) {
            console.log(error);
        }

        this.setLoadingInitial(false);
    }

    selectDuty = (id: string) => {
        this.selectedDuty = this.duties.find(duty => duty.id === id);
    }

    cancelSelectedDuty = () => {
        this.selectedDuty = undefined;
    }

    openDeleteMode = (id: string) => {
        this.setDeleteMode(true);
        this.selectDuty(id);
    }

    closeDeleteMode = (id?: string) => {
        if (id) {
            this.duties = this.duties.filter(duty => duty.id !== id);
        }

        this.setDeleteMode(false);
        this.cancelSelectedDuty();
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    setDeleteMode = (state: boolean) => {
        this.deleteMode = state;
    }
}