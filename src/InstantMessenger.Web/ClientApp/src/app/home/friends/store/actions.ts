import {createAction, props} from '@ngrx/store';
import {FriendshipInterface} from 'src/app/home/friends/types/friendship.interface';
import {InvitationFullInterface} from 'src/app/home/friends/types/invitation.interface';

export enum ActionTypes {
  GET_PENDING_INVITATION = '[Friends] Get pending invitation',
  GET_PENDING_INVITATION_SUCCESS = '[Friends] Get pending invitation success',
  GET_PENDING_INVITATION_FAILURE = '[Friends] Get pending invitation failure',

  GET_FRIENDS = '[Friends] Get friends',
  GET_FRIENDS_SUCCESS = '[Friends] Get friends success',
  GET_FRIENDS_FAILURE = '[Friends] Get friends failure',

  SEND_INVITATION = '[Friends] Send invitation',
  SEND_INVITATION_SUCCESS = '[Friends] Send invitation success',
  SEND_INVITATION_FAILURE = '[Friends] Send invitation failure',

  ACCEPT_INVITATION = '[Friends] Accept invitation',
  ACCEPT_INVITATION_SUCCESS = '[Friends] Accept invitation success',
  ACCEPT_INVITATION_FAILURE = '[Friends] Accept invitation failure',

  REJECT_INVITATION = '[Friends] Reject invitation',
  REJECT_INVITATION_SUCCESS = '[Friends] Reject invitation success',
  REJECT_INVITATION_FAILURE = '[Friends] Reject invitation failure',

  CANCEL_INVITATION = '[Friends] Cancel invitation',
  CANCEL_INVITATION_SUCCESS = '[Friends] Cancel invitation success',
  CANCEL_INVITATION_FAILURE = '[Friends] Cancel invitation failure',

  REMOVE_FRIEND = '[Friends] Remove friend',
  REMOVE_FRIEND_SUCCESS = '[Friends] Remove friend success',
  REMOVE_FRIEND_FAILURE = '[Friends] Remove friend failure',
}

export const getPendingInvitationsAction = createAction(
  ActionTypes.GET_PENDING_INVITATION
);

export const getPendingInvitationsSuccessAction = createAction(
  ActionTypes.GET_PENDING_INVITATION_SUCCESS,
  props<{invitations: InvitationFullInterface[]}>()
);

export const getPendingInvitationsFailureAction = createAction(
  ActionTypes.GET_PENDING_INVITATION_FAILURE
);

export const getFriendsAction = createAction(ActionTypes.GET_FRIENDS);

export const getFriendsSuccessAction = createAction(
  ActionTypes.GET_FRIENDS_SUCCESS,
  props<{friends: FriendshipInterface[]}>()
);

export const getFriendsFailureAction = createAction(
  ActionTypes.GET_FRIENDS_FAILURE
);

export const sendInvitationAction = createAction(
  ActionTypes.SEND_INVITATION,
  props<{nickname: string}>()
);

export const sendInvitationSuccessAction = createAction(
  ActionTypes.SEND_INVITATION_SUCCESS
);

export const sendInvitationFailureAction = createAction(
  ActionTypes.SEND_INVITATION_FAILURE
);

export const acceptInvitationAction = createAction(
  ActionTypes.ACCEPT_INVITATION,
  props<{id: string}>()
);

export const acceptInvitationSuccessAction = createAction(
  ActionTypes.ACCEPT_INVITATION_SUCCESS,
  props<{id: string}>()
);

export const acceptInvitationFailureAction = createAction(
  ActionTypes.ACCEPT_INVITATION_FAILURE
);

export const rejectInvitationAction = createAction(
  ActionTypes.REJECT_INVITATION,
  props<{id: string}>()
);

export const rejectInvitationSuccessAction = createAction(
  ActionTypes.REJECT_INVITATION_SUCCESS,
  props<{id: string}>()
);

export const rejectInvitationFailureAction = createAction(
  ActionTypes.REJECT_INVITATION_FAILURE
);

export const cancelInvitationAction = createAction(
  ActionTypes.CANCEL_INVITATION,
  props<{id: string}>()
);

export const cancelInvitationSuccessAction = createAction(
  ActionTypes.CANCEL_INVITATION_SUCCESS,
  props<{id: string}>()
);

export const cancelInvitationFailureAction = createAction(
  ActionTypes.CANCEL_INVITATION_FAILURE
);

export const removeFriendAction = createAction(
  ActionTypes.REMOVE_FRIEND,
  props<{friendshipId: string}>()
);

export const removeFriendSuccessAction = createAction(
  ActionTypes.REMOVE_FRIEND_SUCCESS,
  props<{friendshipId: string}>()
);

export const removeFriendFailureAction = createAction(
  ActionTypes.REMOVE_FRIEND_FAILURE
);
