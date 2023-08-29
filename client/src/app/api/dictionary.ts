import {
  CreateDictionaryCategoryDto,
  CreateDictionaryItemDto,
  DictionaryCategory,
  DictionaryItem,
  EditDictionaryCategoryDto,
  EditDictionaryItemDto,
  SortDictionaryCategoryDto,
  SortDictionaryItemDto
} from '../models/dictionary';
import { requests } from './agent';

export const DictionaryCategories = {
  list: () => requests.get<DictionaryCategory[]>('/dictionary/categories'),
  create: (category: CreateDictionaryCategoryDto) => requests
    .post<DictionaryCategory>('/dictionary/categories', category),
  update: (category: EditDictionaryCategoryDto) => requests
    .put<DictionaryCategory>(`/dictionary/categories/${category.id}`, category),
  delete: (id: string) => requests.delete<void>(`/dictionary/categories/${id}`),
  sort: (categories: SortDictionaryCategoryDto[]) => requests.patch<void>('/dictionary/categories', categories)
};

export const DictionaryItems = {
  list: () => requests.get<DictionaryItem[]>('/dictionary/items'),
  create: (item: CreateDictionaryItemDto) => requests
    .post<DictionaryItem>('/dictionary/items', item),
  update: (item: EditDictionaryItemDto) => requests
    .put<DictionaryItem>(`/dictionary/items/${item.id}`, item),
  delete: (id: string) => requests.delete<void>(`/dictionary/items/${id}`),
  sort: (items: SortDictionaryItemDto[]) => requests.patch<void>('/dictionary/items', items)
};