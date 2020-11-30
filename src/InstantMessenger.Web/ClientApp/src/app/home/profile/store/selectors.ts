import {createFeatureSelector, createSelector} from '@ngrx/store';
import {ProfilesState} from 'src/app/home/profile/types/ProfilesState.interface';
import {currentUser} from 'src/app/identity/store/selectors';
import {AppStateInterface} from 'src/app/shared/types/appState.interface';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';

export const profilesFeatureSelector = createFeatureSelector<
  AppStateInterface,
  ProfilesState
>('profiles');

export const nicknameSelector = createSelector(
  currentUser,
  (s: CurrentUserInterface): string => s.nickname
);

export const avatarSelector = createSelector(
  profilesFeatureSelector,
  (s): string => (s.avatar ? `${s.avatar}` : 'assets/profile-placeholder.png')
);
