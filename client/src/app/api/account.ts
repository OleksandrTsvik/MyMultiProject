import { User, UserLogin, UserRegister } from '../models/user';
import { requests } from './agent';

export const Account = {
  current: () => requests.get<User>('/account'),
  login: (user: UserLogin) => requests.post<User>('/account/login', user),
  register: (user: UserRegister) => requests.post<User>('/account/register', user),
  refreshToken: () => requests.post<User>('/account/refreshToken')
};