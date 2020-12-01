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
  (state: FriendsStateInterface) =>
    state.pendingInvitations.map((x) => ({
      ...x,
      receiver: {
        ...x.receiver,
        avatar: x.receiver.avatar
          ? x.receiver.avatar
          : 'assets/profile-placeholder.png',
      },
      sender: {
        ...x.sender,
        avatar: x.sender.avatar
          ? x.sender.avatar
          : 'assets/profile-placeholder.png',
      },
    }))
);
