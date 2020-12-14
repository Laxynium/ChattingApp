import {createAction, props} from '@ngrx/store';
import {
  ChannelDto,
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';

import {
  ExpirationTimeType,
  InvitationDto,
  UsageCounterType,
} from 'src/app/home/groups/store/types/invitation';

export enum ActionTypes {
  GET_GROUPS = '[Groups] Get groups',
  GET_GROUPS_SUCCESS = '[Groups] Get groups success',
  GET_GROUPS_FAILURE = '[Groups] Get groups failure',

  GET_CHANNELS = '[Groups] Get channels',
  GET_CHANNELS_SUCCESS = '[Groups] Get channels success',
  GET_CHANNELS_FAILURE = '[Groups] Get channels failure',

  CREATE_GROUP = '[Groups] Create group',
  CREATE_GROUP_SUCCESS = '[Groups] Create group success',
  CREATE_GROUP_FAILURE = '[Groups] Create group failure',

  REMOVE_GROUP = '[Groups] Remove group',
  REMOVE_GROUP_SUCCESS = '[Groups] Remove group success',
  REMOVE_GROUP_FAILURE = '[Groups] Remove group failure',

  JOIN_GROUP = '[Groups] Join group',
  JOIN_GROUP_SUCCESS = '[Groups] Join group success',
  JOIN_GROUP_FAILURE = '[Groups] Join group failure',

  CHANGE_CURRENT_GROUP = '[Groups] Change current group',
  CHANGE_CURRENT_GROUP_SUCCESS = '[Groups] Change current group success',
  CHANGE_CURRENT_GROUP_FAILURE = '[Groups] Change current group failure',

  LOAD_CURRENT_GROUP = '[Groups] Load current group',
  LOAD_CURRENT_GROUP_SUCCESS = '[Groups] Load current group success',
  LOAD_CURRENT_GROUP_FAILURE = '[Groups] Load current group failure',

  CREATE_CHANNEL = '[Groups] Create channel',
  CREATE_CHANNEL_SUCCESS = '[Groups] Create channel success',
  CREATE_CHANNEL_FAILURE = '[Groups] Create channel failure',

  REMOVE_CHANNEL = '[Groups] Remove channel',
  REMOVE_CHANNEL_SUCCESS = '[Groups] Remove channel success',
  REMOVE_CHANNEL_FAILURE = '[Groups] Remove channel failure',

  CHANGE_CURRENT_CHANNEL = '[Groups] Change current channel',
  CHANGE_CURRENT_CHANNEL_SUCCESS = '[Groups] Change current channel success',
  CHANGE_CURRENT_CHANNEL_FAILURE = '[Groups] Change current channel failure',

  LOAD_CURRENT_CHANNEL = '[Groups] Load current channel',
  LOAD_CURRENT_CHANNEL_SUCCESS = '[Groups] Load current channel success',
  LOAD_CURRENT_CHANNEL_FAILURE = '[Groups] Load current channel failure',

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

export const getGroupsAction = createAction(ActionTypes.GET_GROUPS);
export const getGroupsSuccessAction = createAction(
  ActionTypes.GET_GROUPS_SUCCESS,
  props<{groups: GroupDto[]}>()
);
export const getGroupsFailureAction = createAction(
  ActionTypes.GET_GROUPS_FAILURE
);

export const getChannelsAction = createAction(
  ActionTypes.GET_CHANNELS,
  props<{groupId: string}>()
);
export const getChannelsSuccessAction = createAction(
  ActionTypes.GET_CHANNELS_SUCCESS,
  props<{channels: ChannelDto[]}>()
);
export const getChannelsFailureAction = createAction(
  ActionTypes.GET_CHANNELS_FAILURE
);

export const createGroupAction = createAction(
  ActionTypes.CREATE_GROUP,
  props<{groupName: string}>()
);
export const createGroupSuccessAction = createAction(
  ActionTypes.CREATE_GROUP_SUCCESS,
  props<{group: GroupDto}>()
);
export const createGroupFailureAction = createAction(
  ActionTypes.CREATE_GROUP_FAILURE
);

export const removeGroupAction = createAction(
  ActionTypes.REMOVE_GROUP,
  props<{groupId: string}>()
);
export const removeGroupSuccessAction = createAction(
  ActionTypes.REMOVE_GROUP_SUCCESS,
  props<{groupId: string}>()
);
export const removeGroupFailureAction = createAction(
  ActionTypes.REMOVE_GROUP_FAILURE
);

export const joinGroupAction = createAction(
  ActionTypes.JOIN_GROUP,
  props<{invitationCode: string}>()
);
export const joinGroupSuccessAction = createAction(
  ActionTypes.JOIN_GROUP_SUCCESS
);
export const joinGroupFailureAction = createAction(
  ActionTypes.JOIN_GROUP_FAILURE
);

export const changeCurrentGroupAction = createAction(
  ActionTypes.CHANGE_CURRENT_GROUP,
  props<{groupId: string}>()
);
export const changeCurrentGroupSuccessAction = createAction(
  ActionTypes.CHANGE_CURRENT_GROUP_SUCCESS,
  props<{groupId: string}>()
);
export const changeCurrentGroupFailureAction = createAction(
  ActionTypes.CHANGE_CURRENT_GROUP_FAILURE
);

export const loadCurrentGroupAction = createAction(
  ActionTypes.LOAD_CURRENT_GROUP,
  props<{groupId: string}>()
);
export const loadCurrentGroupSuccessAction = createAction(
  ActionTypes.LOAD_CURRENT_GROUP_SUCCESS,
  props<{group: GroupDto}>()
);
export const loadCurrentGroupFailureAction = createAction(
  ActionTypes.LOAD_CURRENT_GROUP_FAILURE
);

export const createChannelAction = createAction(
  ActionTypes.CREATE_CHANNEL,
  props<{groupId: string; channelName: string}>()
);
export const createChannelSuccessAction = createAction(
  ActionTypes.CREATE_CHANNEL_SUCCESS,
  props<{channel: ChannelDto}>()
);
export const createChannelFailureAction = createAction(
  ActionTypes.CREATE_CHANNEL_FAILURE
);

export const removeChannelAction = createAction(
  ActionTypes.REMOVE_CHANNEL,
  props<{groupId: string; channelId: string}>()
);
export const removeChannelSuccessAction = createAction(
  ActionTypes.REMOVE_CHANNEL_SUCCESS,
  props<{channelId: string}>()
);
export const removeChannelFailureAction = createAction(
  ActionTypes.REMOVE_CHANNEL_FAILURE
);

export const changeCurrentChannelAction = createAction(
  ActionTypes.CHANGE_CURRENT_CHANNEL,
  props<{groupId: string; channelId: string}>()
);

export const loadCurrentChannelAction = createAction(
  ActionTypes.LOAD_CURRENT_CHANNEL,
  props<{channelId: string}>()
);
export const loadCurrentChannelSuccessAction = createAction(
  ActionTypes.LOAD_CURRENT_CHANNEL_SUCCESS,
  props<{channelId: string}>()
);
export const loadCurrentChannelFailureAction = createAction(
  ActionTypes.LOAD_CURRENT_CHANNEL_FAILURE
);

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
  props<{invitations: InvitationDto[]}>()
);
export const getInvitationsFailureAction = createAction(
  ActionTypes.GET_INVITATIONS_FAILURE
);
