import {createAction, props} from '@ngrx/store';
import {RoleDto} from 'src/app/home/groups/store/types/role';

export enum ActionTypes {
  GET_ROLES = '[Groups] Get roles',
  GET_ROLES_SUCCESS = '[Groups] Get roles success',
  GET_ROLES_FAILURE = '[Groups] Get roles failure',

  CREATE_ROLE = '[Groups] Create role',
  CREATE_ROLE_SUCCESS = '[Groups] Create role success',
  CREATE_ROLE_FAILURE = '[Groups] Create role failure',

  REMOVE_ROLE = '[Groups] Remove role',
  REMOVE_ROLE_SUCCESS = '[Groups] Remove role success',
  REMOVE_ROLE_FAILURE = '[Groups] Remove role failure',

  ADD_PERMISSION = '[Groups] Add permission',
  ADD_PERMISSION_SUCCESS = '[Groups] Add permission success',
  ADD_PERMISSION_FAILURE = '[Groups] Add permission failure',

  REMOVE_PERMISSION = '[Groups] Remove permission',
  REMOVE_PERMISSION_SUCCESS = '[Groups] Remove permission success',
  REMOVE_PERMISSION_FAILURE = '[Groups] Remove permission failure',
}

export const getRolesAction = createAction(
  ActionTypes.GET_ROLES,
  props<{groupId: string}>()
);
export const getRolesSuccessAction = createAction(
  ActionTypes.GET_ROLES_SUCCESS,
  props<{roles: RoleDto[]}>()
);
export const getRolesFailureAction = createAction(
  ActionTypes.GET_ROLES_FAILURE
);

export const createRoleAction = createAction(
  ActionTypes.CREATE_ROLE,
  props<{groupId: string; roleName: string}>()
);
export const createRoleSuccessAction = createAction(
  ActionTypes.CREATE_ROLE_SUCCESS,
  props<{role: RoleDto}>()
);
export const createRoleFailureAction = createAction(
  ActionTypes.CREATE_ROLE_FAILURE
);

export const removeRoleAction = createAction(
  ActionTypes.REMOVE_ROLE,
  props<{groupId: string; roleId: string}>()
);
export const removeRoleSuccessAction = createAction(
  ActionTypes.REMOVE_ROLE_SUCCESS,
  props<{groupId: string; roleId: string}>()
);
export const removeRoleFailureAction = createAction(
  ActionTypes.REMOVE_ROLE_FAILURE
);

export const addPermissionAction = createAction(
  ActionTypes.ADD_PERMISSION,
  props<{roleName: string}>()
);
export const addPermissionSuccessAction = createAction(
  ActionTypes.ADD_PERMISSION_SUCCESS
);
export const addPermissionFailureAction = createAction(
  ActionTypes.ADD_PERMISSION_FAILURE
);

export const removePermissionAction = createAction(
  ActionTypes.REMOVE_PERMISSION,
  props<{roleName: string}>()
);
export const removePermissionSuccessAction = createAction(
  ActionTypes.REMOVE_PERMISSION_SUCCESS
);
export const removePermissionFailureAction = createAction(
  ActionTypes.REMOVE_PERMISSION_FAILURE
);
