import {Action, createReducer, on} from '@ngrx/store';
import {
  acceptInvitationAction,
  acceptInvitationFailureAction,
  acceptInvitationSuccessAction,
  rejectInvitationAction,
  rejectInvitationFailureAction,
  rejectInvitationSuccessAction,
  cancelInvitationAction,
  cancelInvitationFailureAction,
  cancelInvitationSuccessAction,
  getPendingInvitationsAction,
  getPendingInvitationsFailureAction,
  getPendingInvitationsSuccessAction,
  sendInvitationAction,
  sendInvitationFailureAction,
  sendInvitationSuccessAction,
  getFriendsAction,
  getFriendsSuccessAction,
  getFriendsFailureAction,
  removeFriendSuccessAction,
} from 'src/app/home/friends/store/actions';
import {FriendshipInterface} from 'src/app/home/friends/types/friendship.interface';
import {InvitationFullInterface} from 'src/app/home/friends/types/invitation.interface';

export interface FriendsStateInterface {
  isSubmitting: boolean;
  pendingInvitations: InvitationFullInterface[];
  arePendingInvitationsLoading: boolean;
  friendships: FriendshipInterface[];
  areFriendsLoading: boolean;
}

const initialState: FriendsStateInterface = {
  isSubmitting: false,
  pendingInvitations: [],
  arePendingInvitationsLoading: false,
  friendships: [],
  areFriendsLoading: false,
};

const friendReducer = createReducer(
  initialState,
  on(
    getPendingInvitationsAction,
    (state): FriendsStateInterface => ({
      ...state,
      arePendingInvitationsLoading: true,
    })
  ),
  on(
    getPendingInvitationsSuccessAction,
    (state, action): FriendsStateInterface => ({
      ...state,
      arePendingInvitationsLoading: false,
      pendingInvitations: action.invitations,
    })
  ),
  on(
    getPendingInvitationsFailureAction,
    (state): FriendsStateInterface => ({
      ...state,
      arePendingInvitationsLoading: false,
    })
  ),

  on(
    getFriendsAction,
    (state): FriendsStateInterface => ({
      ...state,
      areFriendsLoading: true,
    })
  ),
  on(
    getFriendsSuccessAction,
    (state, action): FriendsStateInterface => ({
      ...state,
      areFriendsLoading: false,
      friendships: action.friends,
    })
  ),
  on(
    getFriendsFailureAction,
    (state): FriendsStateInterface => ({
      ...state,
      areFriendsLoading: false,
    })
  ),

  on(
    sendInvitationAction,
    (state): FriendsStateInterface => ({
      ...state,
      isSubmitting: true,
    })
  ),
  on(
    sendInvitationSuccessAction,
    (state): FriendsStateInterface => ({
      ...state,
      isSubmitting: false,
    })
  ),
  on(
    sendInvitationFailureAction,
    (state): FriendsStateInterface => ({
      ...state,
      isSubmitting: false,
    })
  ),
  on(
    acceptInvitationAction,
    (state): FriendsStateInterface => ({
      ...state,
    })
  ),
  on(
    acceptInvitationSuccessAction,
    (state, action): FriendsStateInterface => ({
      ...state,
      pendingInvitations: state.pendingInvitations.filter(
        (x) => x.invitationId !== action.id
      ),
    })
  ),
  on(
    acceptInvitationFailureAction,
    (state): FriendsStateInterface => ({
      ...state,
    })
  ),
  on(
    rejectInvitationAction,
    (state): FriendsStateInterface => ({
      ...state,
    })
  ),
  on(
    rejectInvitationSuccessAction,
    (state, action): FriendsStateInterface => ({
      ...state,
      pendingInvitations: state.pendingInvitations.filter(
        (x) => x.invitationId !== action.id
      ),
    })
  ),
  on(
    rejectInvitationFailureAction,
    (state): FriendsStateInterface => ({
      ...state,
    })
  ),
  on(
    cancelInvitationAction,
    (state): FriendsStateInterface => ({
      ...state,
    })
  ),
  on(
    cancelInvitationSuccessAction,
    (state, action): FriendsStateInterface => ({
      ...state,
      pendingInvitations: state.pendingInvitations.filter(
        (x) => x.invitationId !== action.id
      ),
    })
  ),
  on(
    cancelInvitationFailureAction,
    (state): FriendsStateInterface => ({
      ...state,
    })
  ),
  on(
    removeFriendSuccessAction,
    (state, action): FriendsStateInterface => ({
      ...state,
      friendships: state.friendships.filter((x) => x.id != action.friendshipId),
    })
  )
);

export function reducers(state: FriendsStateInterface, action: Action) {
  return friendReducer(state, action);
}
