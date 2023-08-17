export interface DictionaryCategory {
  title: string;
  language: string;
  position: number;
  dateCreation: Date;
  countItems: number;
}

export interface DictionaryItem {
  text: string;
  translation: string;
  status?: string;
  position: number;
  dateCreation: Date;
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