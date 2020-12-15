import {createAction, props} from '@ngrx/store';
import {PermissionOverrideDto} from 'src/app/home/groups/store/types/role-permission-override';

export enum ActionTypes {
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
}

export const updateChannelRolePermissionOverridesAction = createAction(
  ActionTypes.UPDATE_CHANNEL_ROLE_PERMISSION_OVERRIDES,
  props<{
    groupId: string;
    channelId: string;
    roleId: string;
    overrides: PermissionOverrideDto[];
  }>()
);
export const updateChannelRolePermissionOverridesSuccessAction = createAction(
  ActionTypes.UPDATE_CHANNEL_ROLE_PERMISSION_OVERRIDES_SUCCESS,
  props<{
    groupId: string;
    channelId: string;
    roleId: string;
    overrides: PermissionOverrideDto[];
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
    overrides: PermissionOverrideDto[];
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
    overrides: PermissionOverrideDto[];
  }>()
);
export const updateChannelMemberPermissionOverridesSuccessAction = createAction(
  ActionTypes.UPDATE_CHANNEL_MEMBER_PERMISSION_OVERRIDES_SUCCESS,
  props<{
    groupId: string;
    channelId: string;
    memberUserId: string;
    overrides: PermissionOverrideDto[];
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
    overrides: PermissionOverrideDto[];
  }>()
);
export const getChannelMemberPermissionOverridesFailureAction = createAction(
  ActionTypes.GET_CHANNEL_MEMBER_PERMISSION_OVERRIDES_FAILURE
);
