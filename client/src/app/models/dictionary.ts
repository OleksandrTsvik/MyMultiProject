export interface DictionaryCategory {
  id: string;
  title: string;
  language: string;
  position: number;
  dateCreation: Date;
  countItems: number;
}

export interface CreateDictionaryCategoryDto {
  title: string;
  language: string;
}

export interface EditDictionaryCategoryDto {
  id: string;
  title: string;
  language: string;
}

export interface SortDictionaryCategoryDto {
  id: string;
  position: number;
}

export interface DictionaryItem {
  id: string;
  text: string;
  translation: string;
  status?: string;
  dateCreation: Date;
  position: number;
}

export interface CreateDictionaryItemDto {
  categoryId: string;
  text: string;
  translation: string;
  status?: string;
}

export interface EditDictionaryItemDto {
  id: string;
  text: string;
  translation: string;
  status?: string;
}

export interface SortDictionaryItemDto {
  itemId: string;
  categoryId: string;
  position: number;
}

export interface GrammarRuleDto {
  title: string;
  language: string;
  status?: string;
  position: number;
  dateCreation: Date;
}

export interface GrammarRule {
  title: string;
  description: string;
  language: string;
  status?: string;
  dateCreation: Date;
  exampleItems: DictionaryItem[];
}