import { makeAutoObservable, runInAction } from 'mobx';

import {
  CreateDictionaryCategoryDto,
  CreateDictionaryItemDto,
  DictionaryCategory,
  DictionaryItem,
  DictionaryQuantity,
  EditDictionaryCategoryDto,
  EditDictionaryItemDto,
  SortDictionaryCategoryDto
} from '../models/dictionary';
import agent from '../api/agent';

export default class DictionaryStore {
  quantity?: DictionaryQuantity;
  loadingQuantity: boolean = true;

  categories: Map<string, DictionaryCategory> = new Map<string, DictionaryCategory>();
  loadingCategories: boolean = true;

  categoryDetails?: DictionaryCategory;
  loadingCategoryDetails: boolean = true;

  items: Map<string, DictionaryItem> = new Map<string, DictionaryItem>();
  loadingItems: boolean = true;

  constructor() {
    makeAutoObservable(this);
  }

  get categoriesArray(): DictionaryCategory[] {
    return Array.from(this.categories.values());
  }

  get categoriesSortByPosition(): DictionaryCategory[] {
    return this.categoriesArray
      .sort((category01, category02) => category01.position - category02.position);
  }

  get itemsArray(): DictionaryItem[] {
    return Array.from(this.items.values());
  }

  get itemsSortByPosition(): DictionaryItem[] {
    return this.itemsArray
      .sort((item01, item02) => item01.position - item02.position);
  }

  loadQuantity = async () => {
    this.setLoadingQuantity(true);

    try {
      const quantity = await agent.Dictionary.quantity();

      runInAction(() => {
        this.quantity = quantity;
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingQuantity(false);
  }

  updateQuantity = (num: number, key: keyof DictionaryQuantity) => {
    if (!this.quantity) {
      return;
    }

    this.quantity[key] += num;
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

      this.updateQuantity(1, 'countCategories');
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

      this.updateQuantity(-1, 'countCategories');
    } catch (error) {
      console.log(error);
    }
  }

  changePositions = async (sourceIndex: number, destinationIndex: number) => {
    if (sourceIndex === destinationIndex) {
      return;
    }

    let position = 1;
    const sortedCategoriesDto: SortDictionaryCategoryDto[] = [];
    const sortedCategories = this.categoriesArray;

    // remove the element with the source index and insert before the destination index
    sortedCategories.splice(destinationIndex, 0, sortedCategories.splice(sourceIndex, 1)[0]);

    for (const category of sortedCategories) {
      category.position = position;

      this.categories.set(category.id, category);

      sortedCategoriesDto.push({
        id: category.id,
        position: category.position
      });

      position++;
    }

    try {
      await agent.DictionaryCategories.sort(sortedCategoriesDto);
    } catch (error) {
      console.log(error);
    }
  }

  resetCategories = () => {
    this.categories.clear();
    this.setLoadingCategories(true);
  }

  loadCategoryDetails = async (id: string) => {
    this.setLoadingCategoryDetails(true);

    try {
      const categoryDetails = await agent.DictionaryCategories.details(id);

      runInAction(() => {
        this.categoryDetails = categoryDetails;
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingCategoryDetails(false);
  }

  resetCategoryDetails = () => {
    this.categoryDetails = undefined;
    this.setLoadingCategoryDetails(true);
  }

  loadItems = async (categoryId: string) => {
    this.setLoadingItems(true);

    try {
      const items = await agent.DictionaryItems.list(categoryId);

      runInAction(() => {
        for (const item of items) {
          item.dateCreation = new Date(item.dateCreation);

          this.items.set(item.id, item);
        }
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingItems(false);
  }

  createItem = async (item: CreateDictionaryItemDto) => {
    try {
      await agent.DictionaryItems.create(item);
    } catch (error) {
      console.log(error);

      throw error;
    }
  }

  editItem = async (item: EditDictionaryItemDto) => {
    try {
      await agent.DictionaryItems.update(item);
    } catch (error) {
      console.log(error);

      throw error;
    }
  }

  deleteItem = async (id: string) => {
    try {
      await agent.DictionaryItems.delete(id);

      runInAction(() => {
        this.items.delete(id);
      });

      this.updateCountItems(-1);
    } catch (error) {
      console.log(error);
    }
  }

  updateCountItems = (num: number) => {
    if (!this.categoryDetails) {
      return;
    }

    this.categoryDetails.countItems += num;
  }

  resetItems = () => {
    this.items.clear();
    this.setLoadingItems(true);
  }

  setLoadingQuantity = (state: boolean) => {
    this.loadingQuantity = state;
  }

  setLoadingCategories = (state: boolean) => {
    this.loadingCategories = state;
  }

  setLoadingCategoryDetails = (state: boolean) => {
    this.loadingCategoryDetails = state;
  }

  setLoadingItems = (state: boolean) => {
    this.loadingItems = state;
  }
}