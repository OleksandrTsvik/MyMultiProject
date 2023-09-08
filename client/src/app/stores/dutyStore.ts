import { makeAutoObservable, reaction, runInAction } from 'mobx';
import { v4 as uuid } from 'uuid';

import { Duty } from '../models/duty';
import { Pagination, PagingParams } from '../models/pagination';
import { store } from './store';
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

  titles: string[] = [];
  loadingTitles: boolean = false;

  pagination: Pagination | null = null;
  pagingParams: PagingParams = new PagingParams(1, 20);
  predicate: Map<string, string> = new Map<string, string>();

  constructor() {
    makeAutoObservable(this);

    reaction(
      () => this.predicate.keys(),
      () => {
        this.pagingParams = new PagingParams(1, 20);
        this.duties.clear();

        this.loadDuties();
      }
    );
  }

  get dutiesArray(): Duty[] {
    return Array.from(this.duties.values());
  }

  get dutiesSortByDateCompletion(): Duty[] {
    return this.dutiesArray
      .sort((duty01, duty02) => compareDates(duty01.dateCompletion, duty02.dateCompletion));
  }

  get dutiesSortByPosition(): Duty[] {
    return this.dutiesArray
      .sort((duty01, duty02) => duty02.position - duty01.position);
  }

  get axiosParams(): URLSearchParams {
    const params = new URLSearchParams();

    params.append('pageNumber', this.pagingParams.pageNumber.toString());
    params.append('pageSize', this.pagingParams.pageSize.toString());

    this.predicate.forEach((value, key) => {
      params.append(key, value);
    });

    return params;
  }

  loadDuties = async () => {
    this.setLoadingInitial(true);

    try {
      const result = await agent.Duties.list(this.axiosParams);

      runInAction(() => {
        result.data.forEach((duty) => {
          duty.dateCreation = new Date(duty.dateCreation);

          if (duty.dateCompletion) {
            duty.dateCompletion = new Date(duty.dateCompletion);
          }

          this.duties.set(duty.id, duty);
        });
      });

      this.setPagination(result.pagination);
    } catch (error) {
      console.log(error);
    }

    this.setLoadingInitial(false);
  }

  reorderDuties = (overDuty: Duty, dropDuty: Duty) => {
    let position = 0;

    this.dutiesArray
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

    this.dutiesArray
      .sort((duty01, duty02) => duty01.position - duty02.position)
      .forEach((duty, index) => {
        duty.position = index;
        this.duties.set(duty.id, { ...duty });
      });
  }

  reorderDutiesOnServer = async () => {
    try {
      await agent.Duties.updateList(this.dutiesArray);
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
      tempDuty.dateCompletion = new Date();
    } else {
      tempDuty.position = -1;
      tempDuty.dateCompletion = null;
    }

    try {
      await agent.Duties.update(tempDuty);

      store.userStore.updateCountCompletedDuties(isCompleted ? 1 : -1);

      runInAction(() => {
        this.duties.delete(tempDuty.id);
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
    duty.dateCreation = new Date();

    try {
      await agent.Duties.create(duty);

      store.userStore.updateCountDuties(1, duty.isCompleted);

      runInAction(() => {
        this.duties.set(duty.id, duty);

        if (!this.titles.some((title) => title === duty.title)) {
          this.titles.push(duty.title);
        }
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

        if (!this.titles.some((title) => title === duty.title)) {
          this.titles.push(duty.title);
        }
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

      const tempDuty = this.duties.get(id);

      if (tempDuty) {
        store.userStore.updateCountDuties(-1, tempDuty.isCompleted);
      }

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

  resetDutyStore = () => {
    this.duties = new Map<string, Duty>();
    this.selectedDuty = undefined;
    this.selectedDutyForChangeColor = undefined;
    this.changeColorMode = false;
    this.loadingInitial = true;
    this.arrLoadingDutiesId = [];
  }

  loadTitles = async () => {
    this.setLoadingTitles(true);

    try {
      const titles = await agent.Duties.titles();

      runInAction(() => {
        this.titles = titles;
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingTitles(false);
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

  setPagination = (state: Pagination) => {
    this.pagination = state;
  }

  setPagingParams = (state: PagingParams) => {
    this.pagingParams = state;
  }

  setPredicate = (predicate: string, value: string | boolean) => {
    this.predicate.delete(predicate);
    this.predicate.set(predicate, value.toString());
  }

  setLoadingTitles = (state: boolean) => {
    this.loadingTitles = state;
  }
}