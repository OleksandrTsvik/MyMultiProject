import { Image, ListFollowingsPredicate, Photo, Profile } from '../models/profile';
import { requests, uploadFile } from './agent';

export const Profiles = {
  get: (username: string) => requests.get<Profile>(`/profiles/${username}`),
  uploadPhoto: (file: Blob) => uploadFile<Photo>(file, '/photos'),
  setMainPhoto: (id: string) => requests.post<void>(`/photos/${id}/setMain`, {}),
  deletePhoto: (id: string) => requests.delete<void>(`/photos/${id}`),
  updateFollowing: (username: string) => requests.post<void>(`/follow/${username}`, {}),
  listFollowings: (username: string, predicate: ListFollowingsPredicate) =>
    requests.get<Profile[]>(`/follow/${username}?predicate=${predicate}`),
  uploadImage: (file: Blob) => uploadFile<Image>(file, '/images', 'Image'),
  getImage: (name: string) => requests.get<Blob>(`/images/${name}`, { responseType: 'blob' })
};