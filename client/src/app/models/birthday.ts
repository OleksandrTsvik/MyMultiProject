export interface Birthday {
  id: string;
  fullName: string;
  date: Date;
  note?: string;
  age: number;
  daysUntilBirthday: number;
}

export interface CreateBirthdayDto {
  fullName: string;
  date: Date;
  note?: string | null;
}

export interface EditBirthdayDto {
  id: string;
  fullName: string;
  date: Date;
  note?: string | null;
}
