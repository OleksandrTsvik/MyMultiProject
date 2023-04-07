import { makeAutoObservable, runInAction } from 'mobx';
import { v4 as uuid } from 'uuid';

import { Duty } from '../models/duty';
import agent from '../api/agent';
import compareDates from '../utils/compareDates';

export default class DutyStore {
    duties: Map<string, Duty> = new Map<string, Duty>();

    selectedDuty: Duty | undefined = undefined;
    selectedDutyForChangeColor: Duty | undefined = undefined;

    createMode: boolean = false;
    editMode: boolean = false;
    deleteMode: boolean = false;
    changeColorMode: boolean = false;

    loadingInitial: boolean = true;
    createLoading: boolean = false;
    arrLoadingDutiesId: string[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    get dutiesCompleted(): Duty[] {
        return Array.from(this.duties.values())
            .filter(duty => duty.isCompleted);
    }

    get dutiesNotCompleted(): Duty[] {
        return Array.from(this.duties.values())
            .filter(duty => !duty.isCompleted);
    }

    get dutiesCompletedSortByDateCompletion(): Duty[] {
        return this.dutiesCompleted
            .sort((duty01, duty02) => compareDates(duty01.dateCompletion, duty02.dateCompletion));
    }

    get dutiesNotCompletedSortByPosition(): Duty[] {
        return this.dutiesNotCompleted
            .sort((duty01, duty02) => duty02.position - duty01.position);
    }

    get countCompleted(): number {
        return this.dutiesCompleted.length;
    }

    get countNotCompleted(): number {
        return this.dutiesNotCompleted.length;
    }

    loadDuties = async () => {
        this.setLoadingInitial(true);

        try {
            const duties = await agent.Duties.list();

            runInAction(() => {
                duties.forEach((duty) => {
                    this.duties.set(duty.id, duty);
                });
            });
        } catch (error) {
            console.log(error);
        }

        this.setLoadingInitial(false);
    }

    reorderDuties = (overDuty: Duty, dropDuty: Duty) => {
        let position = 0;

        this.dutiesNotCompleted
            .sort((duty01, duty02) => duty01.position - duty02.position)
            .forEach((duty) => {
                if (duty.id === overDuty.id) {
                    if (overDuty.position - dropDuty.position > 0) {
                        // move to the left
                        overDuty.position = position;
                        dropDuty.position = ++position;
                    } else {
                        // move to the right
                        dropDuty.position = position;
                        overDuty.position = ++position;
                    }

                    this.duties.set(overDuty.id, { ...overDuty });
                    this.duties.set(dropDuty.id, { ...dropDuty });
                } else if (duty.id !== overDuty.id && duty.id !== dropDuty.id) {
                    duty.position = position;
                    this.duties.set(duty.id, { ...duty });
                }

                position++;
            });
    }

    reorderDutiesBySwappingPlaces = (overDuty: Duty, dropDuty: Duty) => {
        const overDutyPosition = overDuty.position;

        overDuty.position = dropDuty.position;
        dropDuty.position = overDutyPosition;

        this.duties.set(overDuty.id, overDuty);
        this.duties.set(dropDuty.id, dropDuty);

        this.dutiesNotCompleted
            .sort((duty01, duty02) => duty01.position - duty02.position)
            .forEach((duty, index) => {
                duty.position = index;
                this.duties.set(duty.id, { ...duty });
            });
    }

    reorderDutiesOnServer = async () => {
        try {
            await agent.Duties.updateList(this.dutiesNotCompleted);
        } catch (error) {
            console.log(error);
        }
    }

    selectDuty = (id: string) => {
        this.selectedDuty = this.duties.get(id);
    }

    cancelSelectedDuty = () => {
        this.selectedDuty = undefined;
    }

    updateIsCompletedDuty = async (duty: Duty, isCompleted: boolean) => {
        this.setLoading(duty.id);

        const tempDuty = { ...duty };
        tempDuty.isCompleted = isCompleted;

        if (isCompleted) {
            tempDuty.dateCompletion = (new Date()).toISOString();
        } else {
            const positions = this.dutiesNotCompleted
                .map(duty => duty.position);

            tempDuty.position = Math.max(...positions) + 1;
            tempDuty.dateCompletion = null;
        }

        try {
            await agent.Duties.update(tempDuty);

            runInAction(() => {
                this.duties.set(tempDuty.id, tempDuty);
            });
        } catch (error) {
            console.log(error);
        }

        this.setLoading(tempDuty.id, true);
    }

    toggleChangeColorMode = (duty: Duty) => {
        if (!this.selectedDutyForChangeColor || this.selectedDutyForChangeColor?.id === duty.id) {
            this.setChangeColorMode(!this.changeColorMode);
        }

        if (this.changeColorMode) {
            this.selectedDutyForChangeColor = duty;
        }
    }

    changeColor = async (duty: Duty) => {
        this.setLoading(duty.id);

        try {
            await agent.Duties.update(duty);

            runInAction(() => {
                this.duties.set(duty.id, duty);
            });
        } catch (error) {
            console.log(error);
        }

        this.setLoading(duty.id, true);
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
        this.createLoading = state;
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

    setChangeColorMode = (state: boolean) => {
        this.changeColorMode = state;

        if (!state) {
            this.selectedDutyForChangeColor = undefined;
        }
    }

    getIsChangeColor = (id: string): boolean => {
        return this.changeColorMode && this.selectedDutyForChangeColor?.id === id;
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