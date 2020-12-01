import {createFeatureSelector, createSelector} from '@ngrx/store';
import {FriendsStateInterface} from 'src/app/home/friends/store/reducers';
import {FriendshipInterface} from 'src/app/home/friends/types/friendship.interface';
import {InvitationFullInterface} from 'src/app/home/friends/types/invitation.interface';
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
  (state: FriendsStateInterface): InvitationFullInterface[] =>
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

export const areFriendsLoadingSelector = createSelector(
  friendsFeatureSelector,
  (state: FriendsStateInterface) => state.areFriendsLoading
);

export const friendSelector = createSelector(
  friendsFeatureSelector,
  (state: FriendsStateInterface): FriendshipInterface[] =>
    state.friendships.map((x) => ({
      ...x,
      friend: {
        ...x.friend,
        avatar: x.friend.avatar
          ? x.friend.avatar
          : 'assets/profile-placeholder.png',
      },
    }))
);
