import {createFeatureSelector, createSelector} from '@ngrx/store';
import {AppStateInterface} from 'src/app/shared/types/appState.interface';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';
import {IdentityStateInterface} from '../types/identityState.interface';

export const identityFeatureSelector = createFeatureSelector<
  AppStateInterface,
  IdentityStateInterface
>('identity');

export const isSubmittingSelector = createSelector(
  identityFeatureSelector,
  (identityState: IdentityStateInterface) => identityState.isSubmitting
);

export const currentUser = createSelector(
  identityFeatureSelector,
  (state: IdentityStateInterface): CurrentUserInterface => state.currentUser
);

export const nicknameSelector = createSelector(
  currentUser,
  (s: CurrentUserInterface): string => s.nickname
);

export const avatarSelector = createSelector(currentUser, (s): string =>
  s.avatar ? `${s.avatar}` : 'assets/profile-placeholder.png'
);
