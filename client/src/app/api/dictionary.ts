import {
  CreateDictionaryCategoryDto,
  CreateDictionaryItemDto,
  DictionaryCategory,
  DictionaryItem,
  DictionaryQuantity,
  EditDictionaryCategoryDto,
  EditDictionaryItemDto,
  SortDictionaryCategoryDto,
  SortDictionaryItemDto
} from '../models/dictionary';
import { requests } from './agent';

export const Dictionary = {
  quantity: () => requests.get<DictionaryQuantity>('/dictionary/quantity')
};

export const DictionaryCategories = {
  list: () => requests.get<DictionaryCategory[]>('/dictionary/categories'),
  details: (id: string) => requests.get<DictionaryCategory>(`/dictionary/categories/${id}`),
  create: (category: CreateDictionaryCategoryDto) => requests
    .post<DictionaryCategory>('/dictionary/categories', category),
  update: (category: EditDictionaryCategoryDto) => requests
    .put<DictionaryCategory>(`/dictionary/categories/${category.id}`, category),
  delete: (id: string) => requests.delete<void>(`/dictionary/categories/${id}`),
  sort: (categories: SortDictionaryCategoryDto[]) => requests.patch<void>('/dictionary/categories', categories)
};

export const DictionaryItems = {
  list: (categoryId: string) => requests.get<DictionaryItem[]>('/dictionary/items', { params: { categoryId } }),
  details: (id: string) => requests.get<DictionaryItem>(`/dictionary/items/${id}`),
  create: (item: CreateDictionaryItemDto) => requests
    .post<DictionaryItem>('/dictionary/items', item),
  update: (item: EditDictionaryItemDto) => requests
    .put<DictionaryItem>(`/dictionary/items/${item.id}`, item),
  delete: (id: string) => requests.delete<void>(`/dictionary/items/${id}`),
  sort: (items: SortDictionaryItemDto[]) => requests.patch<void>('/dictionary/items', items)
};