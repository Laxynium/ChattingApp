import {createAction, props} from '@ngrx/store';
import {ExpirationTimeType, Invitation, UsageCounterType} from "src/app/home/groups/store/invitations/reducer";


export enum ActionTypes {
  GENERATE_INVITATION = '[Groups] Generate invitation',
  GENERATE_INVITATION_SUCCESS = '[Groups] Generate invitation success',
  GENERATE_INVITATION_FAILURE = '[Groups] Generate invitation failure',

  REVOKE_INVITATION = '[Groups] Revoke invitation',
  REVOKE_INVITATION_SUCCESS = '[Groups] Revoke invitation success',
  REVOKE_INVITATION_FAILURE = '[Groups] Revoke invitation failure',

  GET_INVITATIONS = '[Groups] Get invitations code',
  GET_INVITATIONS_SUCCESS = '[Groups] Get invitations code success',
  GET_INVITATIONS_FAILURE = '[Groups] Get invitations code failure',
}

export const generateInvitationAction = createAction(
  ActionTypes.GENERATE_INVITATION,
  props<{
    groupId: string;
    expirationTime: {type: ExpirationTimeType; period: string};
    usageCounter: {type: UsageCounterType; times: number};
  }>()
);
export const generateInvitationSuccessAction = createAction(
  ActionTypes.GENERATE_INVITATION_SUCCESS,
  props<{
    groupId: string;
    invitationId: string;
    code: string;
  }>()
);
export const generateInvitationFailureAction = createAction(
  ActionTypes.GENERATE_INVITATION_FAILURE
);

export const revokeInvitationAction = createAction(
  ActionTypes.REVOKE_INVITATION,
  props<{groupId: string; invitationId: string}>()
);
export const revokeInvitationSuccessAction = createAction(
  ActionTypes.REVOKE_INVITATION_SUCCESS,
  props<{invitationId: string}>()
);
export const revokeInvitationFailureAction = createAction(
  ActionTypes.REVOKE_INVITATION_FAILURE
);

export const getInvitationsAction = createAction(
  ActionTypes.GET_INVITATIONS,
  props<{
    groupId: string;
  }>()
);
export const getInvitationsSuccessAction = createAction(
  ActionTypes.GET_INVITATIONS_SUCCESS,
  props<{invitations: Invitation[]}>()
);
export const getInvitationsFailureAction = createAction(
  ActionTypes.GET_INVITATIONS_FAILURE
);
