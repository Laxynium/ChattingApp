import {createAction, props} from '@ngrx/store';
import {GroupDto} from 'src/app/home/groups/services/responses/group.dto';

export enum ActionTypes {
  GET_GROUPS = '[Groups] Get groups',
  GET_GROUPS_SUCCESS = '[Groups] Get groups success',
  GET_GROUPS_FAILURE = '[Groups] Get groups failure',

  CREATE_GROUP = '[Groups] Create group',
  CREATE_GROUP_SUCCESS = '[Groups] Create group success',
  CREATE_GROUP_FAILURE = '[Groups] Create group failure',

  REMOVE_GROUP = '[Groups] Remove group',
  REMOVE_GROUP_SUCCESS = '[Groups] Remove group success',
  REMOVE_GROUP_FAILURE = '[Groups] Remove group failure',

  LEAVE_GROUP = '[Groups] Leave group',
  LEAVE_GROUP_SUCCESS = '[Groups] Leave group success',
  LEAVE_GROUP_FAILURE = '[Groups] Leave group failure',

  RENAME_GROUP = '[Groups] Rename group',
  RENAME_GROUP_SUCCESS = '[Groups] Rename group success',
  RENAME_GROUP_FAILURE = '[Groups] Rename group failure',

  JOIN_GROUP = '[Groups] Join group',
  JOIN_GROUP_SUCCESS = '[Groups] Join group success',
  JOIN_GROUP_FAILURE = '[Groups] Join group failure',

  CHANGE_CURRENT_GROUP = '[Groups] Change current group',
  CHANGE_CURRENT_GROUP_SUCCESS = '[Groups] Change current group success',
  CHANGE_CURRENT_GROUP_FAILURE = '[Groups] Change current group failure',

  LOAD_CURRENT_GROUP = '[Groups] Load current group',
  LOAD_CURRENT_GROUP_SUCCESS = '[Groups] Load current group success',
  LOAD_CURRENT_GROUP_FAILURE = '[Groups] Load current group failure',
}

export const getGroupsAction = createAction(ActionTypes.GET_GROUPS);
export const getGroupsSuccessAction = createAction(
  ActionTypes.GET_GROUPS_SUCCESS,
  props<{groups: GroupDto[]}>()
);
export const getGroupsFailureAction = createAction(
  ActionTypes.GET_GROUPS_FAILURE
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

export const leaveGroupAction = createAction(
  ActionTypes.LEAVE_GROUP,
  props<{groupId: string}>()
);
export const leaveGroupSuccessAction = createAction(
  ActionTypes.LEAVE_GROUP_SUCCESS,
  props<{groupId: string}>()
);
export const leaveGroupFailureAction = createAction(
  ActionTypes.LEAVE_GROUP_FAILURE
);

export const renameGroupAction = createAction(
  ActionTypes.RENAME_GROUP,
  props<{group: GroupDto}>()
);
export const renameGroupSuccessAction = createAction(
  ActionTypes.RENAME_GROUP_SUCCESS,
  props<{group: GroupDto}>()
);
export const renameGroupFailureAction = createAction(
  ActionTypes.RENAME_GROUP_FAILURE
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
