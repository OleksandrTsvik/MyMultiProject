import { Birthday, CreateBirthdayDto, EditBirthdayDto } from '../models/birthday';
import { requests } from './agent';

export const Birthdays = {
  list: () => requests.get<Birthday[]>('/birthdays'),
  details: (id: string) => requests.get<Birthday>(`/birthdays/${id}`),
  create: (birthday: CreateBirthdayDto) =>
    requests.post<Birthday>('/birthdays', birthday),
  update: (birthday: EditBirthdayDto) =>
    requests.put<Birthday>(`/birthdays/${birthday.id}`, birthday),
  delete: (id: string) => requests.delete<void>(`/birthdays/${id}`),
};
