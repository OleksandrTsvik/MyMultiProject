import { makeAutoObservable, runInAction } from 'mobx';
import { v4 as uuid } from 'uuid';

import { Duty } from '../models/duty';
import agent from '../api/agent';

export default class DutyStore {
    duties: Duty[] = [];
    selectedDuty: Duty | undefined = undefined;

    createMode: boolean = false;
    editMode: boolean = false;
    deleteMode: boolean = false;

    loadingInitial: boolean = false;
    loading: boolean = false;

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

    openCreateMode = () => {
        this.setCreateMode(true);
    }

    createDuty = async (duty: Duty) => {
        this.setLoading(true);

        duty.id = uuid();
        duty.dateCreation = (new Date()).toISOString();

        try {
            await agent.Duties.create(duty);

            runInAction(() => {
                this.duties = [...this.duties, duty];
            });

            this.setLoading(false);
            this.closeCreateMode();
        } catch (error) {
            this.setLoading(false);
            console.log(error);
        }
    }

    closeCreateMode = () => {
        this.setCreateMode(false);
    }

    openEditMode = (id: string) => {
        this.setEditMode(true);
        this.selectDuty(id);
    }

    editDuty = async (duty: Duty) => {
        this.setLoading(true);

        try {
            await agent.Duties.update(duty);

            runInAction(() => {
                this.duties = [...this.duties.filter(task => task.id !== duty.id), duty];
            });

            this.setLoading(false);
            this.closeEditMode();
        } catch (error) {
            this.setLoading(false);
            console.log(error);
        }
    }

    closeEditMode = () => {
        this.setEditMode(false);
        this.cancelSelectedDuty();
    }

    openDeleteMode = (id: string) => {
        this.setDeleteMode(true);
        this.selectDuty(id);
    }

    deleteDuty = async (id: string) => {
        this.setLoading(true);

        try {
            await agent.Duties.delete(id);

            this.setLoading(false);
            this.closeDeleteMode(id);
        } catch (error) {
            this.setLoading(false);
            console.log(error);
        }
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

    setLoading = (state: boolean) => {
        this.loading = state;
    }

    setCreateMode = (state: boolean) => {
        this.createMode = state;
    }

    setEditMode = (state: boolean) => {
        this.editMode = state;
    }

    setDeleteMode = (state: boolean) => {
        this.deleteMode = state;
    }
}