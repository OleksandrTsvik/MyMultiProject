import { makeAutoObservable, runInAction } from 'mobx';
import { v4 as uuid } from 'uuid';

import { Duty } from '../models/duty';
import agent from '../api/agent';
import compareDates from '../utils/compareDates';

export default class DutyStore {
    duties: Map<string, Duty> = new Map<string, Duty>();
    selectedDuty: Duty | undefined = undefined;

    createMode: boolean = false;
    editMode: boolean = false;
    deleteMode: boolean = false;

    loadingInitial: boolean = true;
    createloading: boolean = false;
    arrLoadingDutiesId: string[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    get dutiesCompleted(): Duty[] {
        return Array.from(this.duties.values())
            .filter(duty => duty.isCompleted)
            .sort((duty01, duty02) => compareDates(duty01.dateCompletion, duty02.dateCompletion));
    }

    get dutiesNotCompleted(): Duty[] {
        return Array.from(this.duties.values())
            .filter(duty => !duty.isCompleted)
            .sort((duty01, duty02) => duty02.position - duty01.position);
    }

    get countCompleted(): number {
        return Array.from(this.duties.values())
            .filter(duty => duty.isCompleted).length;
    }

    get countNotCompleted(): number {
        return Array.from(this.duties.values())
            .filter(duty => !duty.isCompleted).length;
    }

    loadDuties = async () => {
        this.setLoadingInitial(true);

        try {
            const duties = await agent.Duties.list();
            duties.forEach((duty) => {
                this.duties.set(duty.id, duty);
            });
        } catch (error) {
            console.log(error);
        }

        this.setLoadingInitial(false);
    }

    selectDuty = (id: string) => {
        this.selectedDuty = this.duties.get(id);
    }

    cancelSelectedDuty = () => {
        this.selectedDuty = undefined;
    }

    updateIsCompletedDuty = (duty: Duty, isCompleted: boolean) => {
        duty.isCompleted = isCompleted;
        duty.dateCompletion = isCompleted ? (new Date()).toISOString() : null;

        this.duties.set(duty.id, duty);
    }

    openCreateMode = () => {
        this.setCreateMode(true);
    }

    createDuty = async (duty: Duty) => {
        this.setCreateLoading(true);

        duty.id = uuid();
        duty.dateCreation = (new Date()).toISOString();

        try {
            await agent.Duties.create(duty);

            runInAction(() => {
                this.duties.set(duty.id, duty);
            });

            this.setCreateLoading(false);
            this.closeCreateMode();
        } catch (error) {
            this.setCreateLoading(false);
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
        this.setLoading(duty.id);

        try {
            await agent.Duties.update(duty);

            runInAction(() => {
                this.duties.set(duty.id, duty);
            });

            this.setLoading(duty.id, true);
            this.closeEditMode(duty.id);
        } catch (error) {
            this.setLoading(duty.id, true);
            console.log(error);
        }
    }

    closeEditMode = (id?: string) => {
        if (id && this.selectedDuty && this.selectedDuty.id !== id) {
            return;
        }

        this.setEditMode(false);
        this.cancelSelectedDuty();
    }

    openDeleteMode = (id: string) => {
        this.setDeleteMode(true);
        this.selectDuty(id);
    }

    deleteDuty = async (id: string) => {
        this.setLoading(id);

        try {
            await agent.Duties.delete(id);

            this.setLoading(id, true);
            this.closeDeleteMode(id);
        } catch (error) {
            this.setLoading(id, true);
            console.log(error);
        }
    }

    closeDeleteMode = (id?: string) => {
        if (id) {
            this.duties.delete(id);
        }

        this.setDeleteMode(false);
        this.cancelSelectedDuty();
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    setCreateLoading = (state: boolean) => {
        this.createloading = state;
    }

    setLoading = (id: string, isDelete: boolean = false) => {
        if (isDelete) {
            this.arrLoadingDutiesId = this.arrLoadingDutiesId.filter((load) => load !== id);
        } else if (!this.arrLoadingDutiesId.includes(id)) {
            this.arrLoadingDutiesId.push(id);
        }
    }

    getIsLoading = (id: string): boolean => {
        return this.arrLoadingDutiesId.includes(id);
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