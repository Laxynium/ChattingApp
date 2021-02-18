import {createAction, props} from '@ngrx/store';
import {Member} from "src/app/home/groups/store/members/member.reducer";
import {GroupId, MemberId, RoleId, UserId} from "src/app/home/groups/store/types";
import {Role} from "src/app/home/groups/store/roles/role.redcuer";

export enum ActionTypes {
  GET_MEMBERS = '[Groups] Get members',
  GET_MEMBERS_SUCCESS = '[Groups] Get members success',
  GET_MEMBERS_FAILURE = '[Groups] Get members failure',

  KICK_MEMBER = '[Groups] Kick member',
  KICK_MEMBER_SUCCESS = '[Groups] Kick member success',
  KICK_MEMBER_FAILURE = '[Groups] Kick member failure',

  ADD_ROLE_TO_MEMBER = '[Groups] Add role to member',
  ADD_ROLE_TO_MEMBER_SUCCESS = '[Groups] Add role to member success',
  ADD_ROLE_TO_MEMBER_FAILURE = '[Groups] Add role to member failure',

  REMOVE_ROLE_FROM_MEMBER = '[Groups] Remove role from member',
  REMOVE_ROLE_FROM_MEMBER_SUCCESS = '[Groups] Remove role from member success',
  REMOVE_ROLE_FROM_MEMBER_FAILURE = '[Groups] Remove role from member failure',

  GET_MEMBER_ROLES = '[Groups] Get member roles',
  GET_MEMBER_ROLES_SUCCESS = '[Groups] Get member roles success',
  GET_MEMBER_ROLES_FAILURE = '[Groups] Get member roles failure',
}

export const getMembersAction = createAction(
  ActionTypes.GET_MEMBERS,
  props<{groupId: string}>()
);
export const getMembersSuccessAction = createAction(
  ActionTypes.GET_MEMBERS_SUCCESS,
  props<{members: Member[]}>()
);
export const getMembersFailureAction = createAction(
  ActionTypes.GET_MEMBERS_FAILURE
);

export const kickMemberAction = createAction(
  ActionTypes.KICK_MEMBER,
  props<{groupId: string; userId: string; memberId: string}>()
);
export const kickMemberSuccessAction = createAction(
  ActionTypes.KICK_MEMBER_SUCCESS,
  props<{groupId: string; userId: string; memberId: string}>()
);
export const kickMemberFailureAction = createAction(
  ActionTypes.KICK_MEMBER_FAILURE
);

export const getMemberRolesAction = createAction(
  ActionTypes.GET_MEMBER_ROLES,
  props<{groupId: string; userId: string, memberId: string}>()
);
export const getMemberRolesSuccessAction = createAction(
  ActionTypes.GET_MEMBER_ROLES_SUCCESS,
  props<{userId: string, memberId:string,roles: Role[]}>()
);
export const getMemberRolesFailureAction = createAction(
  ActionTypes.GET_MEMBER_ROLES_FAILURE
);

export const addRoleToMemberAction = createAction(
  ActionTypes.ADD_ROLE_TO_MEMBER,
  props<{groupId: GroupId, userId: UserId, memberId: MemberId, roleId: RoleId}>()
);
export const addRoleToMemberSuccessAction = createAction(
  ActionTypes.ADD_ROLE_TO_MEMBER_SUCCESS,
  props<{roleId: RoleId, memberId: MemberId, groupId: GroupId, userId: UserId}>()
);
export const addRoleToMemberFailureAction = createAction(
  ActionTypes.ADD_ROLE_TO_MEMBER_FAILURE
);

export const removeRoleFromMemberAction = createAction(
  ActionTypes.REMOVE_ROLE_FROM_MEMBER,
  props<{groupId: GroupId, userId: UserId, memberId: MemberId, roleId: RoleId}>()
);
export const removeRoleFromMemberSuccessAction = createAction(
  ActionTypes.REMOVE_ROLE_FROM_MEMBER_SUCCESS,
  props<{roleId: RoleId, memberId: MemberId}>()
);
export const removeRoleFromMemberFailureAction = createAction(
  ActionTypes.REMOVE_ROLE_FROM_MEMBER_FAILURE
);
