import {Action, createReducer, on} from '@ngrx/store';
import {
  getPendingInvitationsAction,
  getPendingInvitationsFailureAction,
  getPendingInvitationsSuccessAction,
  sendInvitationAction,
  sendInvitationFailureAction,
  sendInvitationSuccessAction,
} from 'src/app/home/friends/store/actions';
import {InvitationFullInterface} from 'src/app/home/friends/types/invitation.interface';

export interface FriendsStateInterface {
  isSubmitting: boolean;
  pendingInvitations: InvitationFullInterface[];
  arePendingInvitationsLoading: boolean;
}

const initialState: FriendsStateInterface = {
  isSubmitting: false,
  pendingInvitations: [],
  arePendingInvitationsLoading: false,
};

const friendReducer = createReducer(
  initialState,
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
  )
);

export function reducers(state: FriendsStateInterface, action: Action) {
  return friendReducer(state, action);
}
