import { makeAutoObservable, runInAction } from 'mobx';

import { ListFollowingsPredicate, Photo, Profile } from '../models/profile';
import agent from '../api/agent';
import updateItemInArray from '../utils/updateItemInArray';
import { store } from './store';

export default class ProfileStore {
  profile: Profile | null = null;
  loadingProfile: boolean = false;

  // Photo
  uploading: boolean = false;
  loadingSetMain: boolean = false;
  loadingDelete: boolean = false;

  followings: Profile[] = [];
  loadingFollow: boolean = false;
  loadingFollowings: boolean = true;

  constructor() {
    makeAutoObservable(this);
  }

  get isCurrentUser(): boolean {
    if (store.userStore.user && this.profile) {
      return store.userStore.user.userName === this.profile.userName;
    }

    return false;
  }

  loadProfile = async (username: string) => {
    this.setLoadingProfile(true);

    try {
      const profile = await agent.Profiles.get(username);

      runInAction(() => {
        this.profile = profile;
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingProfile(false);
  }

  uploadPhoto = async (file: Blob) => {
    this.setUploading(true);

    try {
      const photo = await agent.Profiles.uploadPhoto(file);

      runInAction(() => {
        if (!this.profile) {
          return;
        }

        this.profile.photos.push(photo);

        if (photo.isMain) {
          this.profile.image = photo.url;
          store.userStore.setImage(photo.url);
        }
      });
    } catch (error) {
      console.log(error);
    }

    this.setUploading(false);
  }

  setMainPhoto = async (photo: Photo) => {
    this.setLoadingSetMain(true);

    try {
      await agent.Profiles.setMainPhoto(photo.id);

      runInAction(() => {
        if (!this.profile) {
          return;
        }

        this.profile.image = photo.url;
        store.userStore.setImage(photo.url);

        updateItemInArray(this.profile.photos, { isMain: false }, (p) => p.isMain);
        updateItemInArray(this.profile.photos, { isMain: true }, (p) => p.id === photo.id);
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingSetMain(false);
  }

  deletePhoto = async (photo: Photo) => {
    this.setLoadingDelete(true);

    try {
      await agent.Profiles.deletePhoto(photo.id);

      runInAction(() => {
        if (!this.profile) {
          return;
        }

        this.profile.photos = this.profile.photos.filter((p) => p.id !== photo.id);

        if (photo.isMain && !this.profile.photos.some((p) => p.isMain)) {
          this.profile.image = undefined;
          store.userStore.setImage(undefined);
        }
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingDelete(false);
  }

  updateFollowing = async (username: string, following: boolean) => {
    this.setLoadingFollow(true);

    try {
      await agent.Profiles.updateFollowing(username);

      runInAction(() => {
        if (
          this.profile && this.profile.userName === username &&
          this.profile.userName !== store.userStore.user?.userName
        ) {
          if (following) {
            this.profile.followersCount++;
          } else {
            this.profile.followersCount--;
          }

          this.profile.following = !this.profile.following;
        }

        if (this.profile && this.profile.userName === store.userStore.user?.userName) {
          if (following) {
            this.profile.followingCount++;
          } else {
            this.profile.followingCount--;
          }
        }

        this.followings.forEach((profile) => {
          if (profile.userName === username) {
            if (profile.following) {
              profile.followersCount--;
            } else {
              profile.followersCount++;
            }

            profile.following = !profile.following;
          }
        });
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingFollow(false);
  }

  loadFollowings = async (predicate: ListFollowingsPredicate) => {
    if (!this.profile) {
      return;
    }

    this.setLoadingFollowings(true);

    try {
      let followings = await agent.Profiles.listFollowings(this.profile.userName, predicate);

      runInAction(() => {
        this.followings = followings;
      });
    } catch (error) {
      console.log(error);
    }

    this.setLoadingFollowings(false);
  }

  resetFollowings = () => {
    this.followings = [];
    this.loadingFollowings = true;
  }

  setLoadingProfile = (state: boolean) => {
    this.loadingProfile = state;
  }

  setUploading = (state: boolean) => {
    this.uploading = state;
  }

  setLoadingSetMain = (state: boolean) => {
    this.loadingSetMain = state;
  }

  setLoadingDelete = (state: boolean) => {
    this.loadingDelete = state;
  }

  setLoadingFollow = (state: boolean) => {
    this.loadingFollow = state;
  }

  setLoadingFollowings = (state: boolean) => {
    this.loadingFollowings = state;
  }
}