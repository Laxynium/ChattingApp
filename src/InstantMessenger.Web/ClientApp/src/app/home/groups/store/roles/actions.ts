import {createAction, props} from '@ngrx/store';
import {PermissionDto} from 'src/app/home/groups/store/types/permission';
import {RoleDto} from 'src/app/home/groups/store/types/role';

export enum ActionTypes {
  GET_ROLES = '[Groups] Get roles',
  GET_ROLES_SUCCESS = '[Groups] Get roles success',
  GET_ROLES_FAILURE = '[Groups] Get roles failure',

  GET_ROLE_PERMISSIONS = '[Groups] Get role permissions',
  GET_ROLE_PERMISSIONS_SUCCESS = '[Groups] Get role permissions success',
  GET_ROLE_PERMISSIONS_FAILURE = '[Groups] Get role permissions failure',

  UPDATE_ROLE_PERMISSIONS = '[Groups] Update role permissions',
  UPDATE_ROLE_PERMISSIONS_SUCCESS = '[Groups] Update role permissions success',
  UPDATE_ROLE_PERMISSIONS_FAILURE = '[Groups] Update role permissions failure',

  CREATE_ROLE = '[Groups] Create role',
  CREATE_ROLE_SUCCESS = '[Groups] Create role success',
  CREATE_ROLE_FAILURE = '[Groups] Create role failure',

  REMOVE_ROLE = '[Groups] Remove role',
  REMOVE_ROLE_SUCCESS = '[Groups] Remove role success',
  REMOVE_ROLE_FAILURE = '[Groups] Remove role failure',

  RENAME_ROLE = '[Groups] Rename role',
  RENAME_ROLE_SUCCESS = '[Groups] Rename role success',
  RENAME_ROLE_FAILURE = '[Groups] Rename role failure',

  ADD_PERMISSION = '[Groups] Add permission',
  ADD_PERMISSION_SUCCESS = '[Groups] Add permission success',
  ADD_PERMISSION_FAILURE = '[Groups] Add permission failure',

  REMOVE_PERMISSION = '[Groups] Remove permission',
  REMOVE_PERMISSION_SUCCESS = '[Groups] Remove permission success',
  REMOVE_PERMISSION_FAILURE = '[Groups] Remove permission failure',

  MOVE_UP_ROLE = '[Groups] Move up role',
  MOVE_UP_ROLE_SUCCESS = '[Groups] Move up role success',
  MOVE_UP_ROLE_FAILURE = '[Groups] Move up role failure',

  MOVE_DOWN_ROLE = '[Groups] Move down role',
  MOVE_DOWN_ROLE_SUCCESS = '[Groups] Move down role success',
  MOVE_DOWN_ROLE_FAILURE = '[Groups] Move down role failure',
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

export const getRolePermissionsAction = createAction(
  ActionTypes.GET_ROLE_PERMISSIONS,
  props<{groupId: string; roleId: string}>()
);
export const getRolePermissionsSuccessAction = createAction(
  ActionTypes.GET_ROLE_PERMISSIONS_SUCCESS,
  props<{roles: PermissionDto[]}>()
);
export const getRolePermissionsFailureAction = createAction(
  ActionTypes.GET_ROLE_PERMISSIONS_FAILURE
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

export const updateRolePermissionsAction = createAction(
  ActionTypes.UPDATE_ROLE_PERMISSIONS,
  props<{
    groupId: string;
    roleId: string;
    permissions: PermissionDto[];
  }>()
);
export const updateRolePermissionsSuccessAction = createAction(
  ActionTypes.UPDATE_ROLE_PERMISSIONS_SUCCESS,
  props<{
    groupId: string;
    roleId: string;
    permissions: PermissionDto[];
  }>()
);
export const updateRolePermissionsFailureAction = createAction(
  ActionTypes.UPDATE_ROLE_PERMISSIONS_FAILURE
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

export const renameRoleAction = createAction(
  ActionTypes.RENAME_ROLE,
  props<{role: RoleDto}>()
);
export const renameRoleSuccessAction = createAction(
  ActionTypes.RENAME_ROLE_SUCCESS,
  props<{role: RoleDto}>()
);
export const renameRoleFailureAction = createAction(
  ActionTypes.RENAME_ROLE_FAILURE
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

export const moveUpRoleAction = createAction(
  ActionTypes.MOVE_UP_ROLE,
  props<{groupId: string; roleId: string}>()
);
export const moveUpRoleSuccessAction = createAction(
  ActionTypes.MOVE_UP_ROLE_SUCCESS,
  props<{roles: RoleDto[]}>()
);
export const moveUpRoleFailureAction = createAction(
  ActionTypes.MOVE_UP_ROLE_FAILURE
);

export const moveDownRoleAction = createAction(
  ActionTypes.MOVE_DOWN_ROLE,
  props<{groupId: string; roleId: string}>()
);
export const moveDownRoleSuccessAction = createAction(
  ActionTypes.MOVE_DOWN_ROLE_SUCCESS,
  props<{roles: RoleDto[]}>()
);
export const moveDownRoleFailureAction = createAction(
  ActionTypes.MOVE_DOWN_ROLE_FAILURE
);
