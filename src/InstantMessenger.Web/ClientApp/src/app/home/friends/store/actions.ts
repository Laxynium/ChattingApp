import {createAction, props} from '@ngrx/store';
import {
  InvitationFullInterface,
  InvitationInterface,
} from 'src/app/home/friends/types/invitation.interface';

export enum ActionTypes {
  SEND_INVITATION = '[Friends] Send invitation',
  SEND_INVITATION_SUCCESS = '[Friends] Send invitation success',
  SEND_INVITATION_FAILURE = '[Friends] Send invitation failure',

  GET_PENDING_INVITATION = '[Friends] Get pending invitation',
  GET_PENDING_INVITATION_SUCCESS = '[Friends] Get pending invitation success',
  GET_PENDING_INVITATION_FAILURE = '[Friends] Get pending invitation failure',
}

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
