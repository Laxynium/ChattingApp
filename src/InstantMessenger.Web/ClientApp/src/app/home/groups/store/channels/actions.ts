import {createAction, props} from '@ngrx/store';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {RolePermissionOverride} from "src/app/home/groups/store/channels/channel.override.role.reducer";
import {MemberPermissionOverride} from "src/app/home/groups/store/channels/channel.override.member.reducer";

export enum ActionTypes {
  GET_CHANNELS = '[Groups] Get channels',
  GET_CHANNELS_SUCCESS = '[Groups] Get channels success',
  GET_CHANNELS_FAILURE = '[Groups] Get channels failure',

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

  UPDATE_CHANNEL_ROLE_PERMISSION_OVERRIDES = '[Groups] Update channel role permission overrides',
  UPDATE_CHANNEL_ROLE_PERMISSION_OVERRIDES_SUCCESS = '[Groups] Update channel role permission overrides success',
  UPDATE_CHANNEL_ROLE_PERMISSION_OVERRIDES_FAILURE = '[Groups] Update channel role permission overrides failure',

  GET_CHANNEL_ROLE_PERMISSION_OVERRIDES = '[Groups] Get channel role permission overrides',
  GET_CHANNEL_ROLE_PERMISSION_OVERRIDES_SUCCESS = '[Groups] Get channel role permission overrides success',
  GET_CHANNEL_ROLE_PERMISSION_OVERRIDES_FAILURE = '[Groups] Get channel role permission overrides failure',

  UPDATE_CHANNEL_MEMBER_PERMISSION_OVERRIDES = '[Groups] Update channel member permission overrides',
  UPDATE_CHANNEL_MEMBER_PERMISSION_OVERRIDES_SUCCESS = '[Groups] Update channel member permission overrides success',
  UPDATE_CHANNEL_MEMBER_PERMISSION_OVERRIDES_FAILURE = '[Groups] Update channel member permission overrides failure',

  GET_CHANNEL_MEMBER_PERMISSION_OVERRIDES = '[Groups] Get channel member permission overrides',
  GET_CHANNEL_MEMBER_PERMISSION_OVERRIDES_SUCCESS = '[Groups] Get channel member permission overrides success',
  GET_CHANNEL_MEMBER_PERMISSION_OVERRIDES_FAILURE = '[Groups] Get channel member permission overrides failure',

  RENAME_CHANNEL = '[Groups] Rename channel',
  RENAME_CHANNEL_SUCCESS = '[Groups] Rename channel success',
  RENAME_CHANNEL_FAILURE = '[Groups] Rename channel failure',
}

export const getChannelsAction = createAction(
  ActionTypes.GET_CHANNELS,
  props<{groupId: string}>()
);
export const getChannelsSuccessAction = createAction(
  ActionTypes.GET_CHANNELS_SUCCESS,
  props<{groupId: string; channels: ChannelDto[]}>()
);
export const getChannelsFailureAction = createAction(
  ActionTypes.GET_CHANNELS_FAILURE
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
  props<{groupId: string; channelId: string}>()
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

export const updateChannelRolePermissionOverridesAction = createAction(
  ActionTypes.UPDATE_CHANNEL_ROLE_PERMISSION_OVERRIDES,
  props<{
    groupId: string;
    channelId: string;
    roleId: string;
    overrides: RolePermissionOverride[];
  }>()
);
export const updateChannelRolePermissionOverridesSuccessAction = createAction(
  ActionTypes.UPDATE_CHANNEL_ROLE_PERMISSION_OVERRIDES_SUCCESS,
  props<{
    groupId: string;
    channelId: string;
    roleId: string;
    overrides: RolePermissionOverride[];
  }>()
);
export const updateChannelRolePermissionOverridesFailureAction = createAction(
  ActionTypes.UPDATE_CHANNEL_ROLE_PERMISSION_OVERRIDES_FAILURE
);

export const getChannelRolePermissionOverridesAction = createAction(
  ActionTypes.GET_CHANNEL_ROLE_PERMISSION_OVERRIDES,
  props<{
    groupId: string;
    channelId: string;
    roleId: string;
  }>()
);
export const getChannelRolePermissionOverridesSuccessAction = createAction(
  ActionTypes.GET_CHANNEL_ROLE_PERMISSION_OVERRIDES_SUCCESS,
  props<{
    groupId: string;
    channelId: string;
    roleId: string;
    overrides: RolePermissionOverride[];
  }>()
);
export const getChannelRolePermissionOverridesFailureAction = createAction(
  ActionTypes.GET_CHANNEL_ROLE_PERMISSION_OVERRIDES_FAILURE
);

export const updateChannelMemberPermissionOverridesAction = createAction(
  ActionTypes.UPDATE_CHANNEL_MEMBER_PERMISSION_OVERRIDES,
  props<{
    groupId: string;
    channelId: string;
    memberUserId: string;
    overrides: MemberPermissionOverride[];
  }>()
);
export const updateChannelMemberPermissionOverridesSuccessAction = createAction(
  ActionTypes.UPDATE_CHANNEL_MEMBER_PERMISSION_OVERRIDES_SUCCESS,
  props<{
    groupId: string;
    channelId: string;
    memberUserId: string;
    overrides: MemberPermissionOverride[];
  }>()
);
export const updateChannelMemberPermissionOverridesFailureAction = createAction(
  ActionTypes.UPDATE_CHANNEL_MEMBER_PERMISSION_OVERRIDES_FAILURE
);

export const getChannelMemberPermissionOverridesAction = createAction(
  ActionTypes.GET_CHANNEL_MEMBER_PERMISSION_OVERRIDES,
  props<{
    groupId: string;
    channelId: string;
    memberUserId: string;
  }>()
);
export const getChannelMemberPermissionOverridesSuccessAction = createAction(
  ActionTypes.GET_CHANNEL_MEMBER_PERMISSION_OVERRIDES_SUCCESS,
  props<{
    groupId: string;
    channelId: string;
    memberUserId: string;
    overrides: MemberPermissionOverride[];
  }>()
);
export const getChannelMemberPermissionOverridesFailureAction = createAction(
  ActionTypes.GET_CHANNEL_MEMBER_PERMISSION_OVERRIDES_FAILURE
);

export const renameChannelAction = createAction(
  ActionTypes.RENAME_CHANNEL,
  props<{channel: ChannelDto}>()
);
export const renameChannelSuccessAction = createAction(
  ActionTypes.RENAME_CHANNEL_SUCCESS,
  props<{channel: ChannelDto}>()
);
export const renameChannelFailureAction = createAction(
  ActionTypes.RENAME_CHANNEL_FAILURE
);
