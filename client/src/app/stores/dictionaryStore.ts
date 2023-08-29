import { makeAutoObservable, runInAction } from 'mobx';

import { CreateDictionaryCategoryDto, DictionaryCategory, EditDictionaryCategoryDto } from '../models/dictionary';
import agent from '../api/agent';

export default class DictionaryStore {
  categories: Map<string, DictionaryCategory> = new Map<string, DictionaryCategory>();

  loadingCategories: boolean = true;

  constructor() {
    makeAutoObservable(this);
  }

  get categoriesArray(): DictionaryCategory[] {
    return Array.from(this.categories.values());
  }

  get categoriesSortByPosition(): DictionaryCategory[] {
    return this.categoriesArray
      .sort((category01, category02) => category02.position - category01.position);
  }

  loadCategories = async () => {
    this.setLoadingCategories(true);

    try {
      const categories = await agent.DictionaryCategories.list();

      runInAction(() => {
        for (const category of categories) {
          category.dateCreation = new Date(category.dateCreation);

          this.categories.set(category.id, category);
        }
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingCategories(false);
  }

  createCategory = async (category: CreateDictionaryCategoryDto) => {
    try {
      const addedCategory = await agent.DictionaryCategories.create(category);

      runInAction(() => {
        this.categories.set(addedCategory.id, addedCategory);
      });
    } catch (error) {
      console.log(error);

      throw error;
    }
  }

  editCategory = async (category: EditDictionaryCategoryDto) => {
    try {
      const editedCategory = await agent.DictionaryCategories.update(category);

      runInAction(() => {
        this.categories.set(editedCategory.id, editedCategory);
      });
    } catch (error) {
      console.log(error);

      throw error;
    }
  }

  deleteCategory = async (id: string) => {
    try {
      await agent.DictionaryCategories.delete(id);

      runInAction(() => {
        this.categories.delete(id);
      });
    } catch (error) {
      console.log(error);
    }
  }

  resetCategories = () => {
    this.categories.clear();
    this.setLoadingCategories(true);
  }

  setLoadingCategories = (state: boolean) => {
    this.loadingCategories = state;
  }
}