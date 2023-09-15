export interface DictionaryQuantity {
  countCategories: number;
  countRules: number;
}

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
  status?: string | null;
  dateCreation: Date;
  position: number;
}

export interface CreateDictionaryItemDto {
  categoryId: string;
  text: string;
  translation: string;
  status?: string | null;
}

export interface EditDictionaryItemDto {
  id: string;
  text: string;
  translation: string;
  status?: string | null;
}

export interface SortDictionaryItemDto {
  itemId: string;
  categoryId: string;
  position: number;
}

export interface GrammarRule {
  id: string;
  title: string;
  description: string;
  language: string;
  status?: string;
  position: number;
  dateCreation: Date;
}

export interface GrammarRuleListItem extends Omit<GrammarRule, 'description'> { }

export interface SortGrammarRuleDto {
  id: string;
  position: number;
}

export interface CreateGrammarRuleDto {
  title: string;
  description: string;
  language: string;
  status?: string;
}

export interface EditGrammarRuleDto extends CreateGrammarRuleDto {
  id: string;
}