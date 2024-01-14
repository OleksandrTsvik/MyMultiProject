import { makeAutoObservable, runInAction } from 'mobx';

import {
  Birthday,
  CreateBirthdayDto,
  EditBirthdayDto,
} from '../models/birthday';
import agent from '../api/agent';

export default class BirthdayStore {
  birthdays: Map<string, Birthday> = new Map<string, Birthday>();
  loadingBirthdays: boolean = true;

  constructor() {
    makeAutoObservable(this);
  }

  get birthdaysArray(): Birthday[] {
    return Array.from(this.birthdays.values());
  }

  get birthdaysSortByDaysUntilBirthday(): Birthday[] {
    return this.birthdaysArray.sort(
      (birthday01, birthday02) =>
        birthday01.daysUntilBirthday - birthday02.daysUntilBirthday,
    );
  }

  loadBirthdays = async () => {
    this.setLoadingBirthdays(true);

    try {
      const birthdays = await agent.Birthdays.list();

      runInAction(() => {
        for (const birthday of birthdays) {
          birthday.date = new Date(birthday.date);

          this.birthdays.set(birthday.id, birthday);
        }
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingBirthdays(false);
  };

  createBirthday = async (birthday: CreateBirthdayDto) => {
    try {
      const addedBirthday = await agent.Birthdays.create(birthday);

      runInAction(() => {
        addedBirthday.date = new Date(addedBirthday.date);
        this.birthdays.set(addedBirthday.id, addedBirthday);
      });
    } catch (error) {
      console.log(error);

      throw error;
    }
  };

  editBirthday = async (birthday: EditBirthdayDto) => {
    try {
      const editedBirthday = await agent.Birthdays.update(birthday);

      runInAction(() => {
        editedBirthday.date = new Date(editedBirthday.date);
        this.birthdays.set(editedBirthday.id, editedBirthday);
      });
    } catch (error) {
      console.log(error);

      throw error;
    }
  };

  deleteBirthday = async (id: string) => {
    try {
      await agent.Birthdays.delete(id);

      runInAction(() => {
        this.birthdays.delete(id);
      });
    } catch (error) {
      console.log(error);
    }
  };

  resetBirthdays = () => {
    this.birthdays.clear();
    this.setLoadingBirthdays(true);
  };

  setLoadingBirthdays = (state: boolean) => {
    this.loadingBirthdays = state;
  };
}
