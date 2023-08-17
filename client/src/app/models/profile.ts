export interface Profile {
  userName: string;
  email: string;
  registrationDate: Date;
  image?: string;
  following: boolean;
  followersCount: number;
  followingCount: number;
  photos: Photo[];
}

export interface Photo {
  id: string;
  url: string;
  isMain: boolean;
}

export enum ListFollowingsPredicate {
  followers = 'followers',
  following = 'following'
}