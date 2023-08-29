import { Duty } from '../models/duty';
import { PaginatedResult } from '../models/pagination';
import { requests } from './agent';

export const Duties = {
  list: (params: URLSearchParams) => requests.get<PaginatedResult<Duty[]>>('/duties', { params }),
  details: (id: string) => requests.get<Duty>(`/duties/${id}`),
  create: (duty: Duty) => requests.post<void>('/duties', duty),
  update: (duty: Duty) => requests.put<void>(`/duties/${duty.id}`, duty),
  delete: (id: string) => requests.delete<void>(`/duties/${id}`),
  updateList: (duties: Duty[]) => requests.put<void>('/duties/list', duties)
};