import {createFeatureSelector, createSelector} from '@ngrx/store';
import {FriendsStateInterface} from 'src/app/home/friends/store/reducers';
import {AppStateInterface} from 'src/app/shared/types/appState.interface';

export const friendsFeatureSelector = createFeatureSelector<
  AppStateInterface,
  FriendsStateInterface
>('friends');

export const isSubmittingSelector = createSelector(
  friendsFeatureSelector,
  (state: FriendsStateInterface) => state.isSubmitting
);

export const arePendingInvitationsLoadingSelector = createSelector(
  friendsFeatureSelector,
  (state: FriendsStateInterface) => state.arePendingInvitationsLoading
);

export const pendingInvitationsSelector = createSelector(
  friendsFeatureSelector,
  (state: FriendsStateInterface) => state.pendingInvitations
);
