import {createReducer, on} from '@ngrx/store';
import {
  createRoleAction,
  createRoleFailureAction,
  createRoleSuccessAction,
  getRolesAction,
  getRolesFailureAction,
  getRolesSuccessAction,
  moveDownRoleSuccessAction,
  moveUpRoleSuccessAction,
  removeRoleSuccessAction,
  renameRoleSuccessAction,
} from './roles/actions';
import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {GroupId, RoleId} from './types';

export interface Role {
  groupId: GroupId;
  roleId: RoleId;
  name: string;
  priority: number;
}

export const roleAdapter = createEntityAdapter<Role>({
  selectId: (x) => x.roleId,
});

export interface RolesState extends EntityState<Role> {
  isLoading: boolean;
  current: RoleId;
  isBeingCreated: boolean;
}

export const roleReducer = createReducer(
  roleAdapter.getInitialState({
    isLoading: false,
    current: null,
    isBeingCreated: false,
  }),
  on(getRolesAction, (s) => ({...s, isLoading: true})),
  on(getRolesSuccessAction, (s, a) => ({
    ...roleAdapter.setAll(
      a.roles.map((r) => ({
        roleId: r.roleId,
        groupId: r.groupId,
        name: r.name,
        priority: r.priority,
      })),
      s
    ),
    isLoading: false,
  })),
  on(getRolesFailureAction, (s) => ({...s, isLoading: false})),
  on(createRoleAction, (s) => ({...s, isBeingCreated: true})),
  on(createRoleSuccessAction, (s, {role}) =>
    roleAdapter.addOne(
      {
        roleId: role.roleId,
        groupId: role.groupId,
        name: role.name,
        priority: role.priority,
      },
      s
    )
  ),
  on(createRoleFailureAction, (s) => ({...s, isBeingCreated: false})),
  on(removeRoleSuccessAction, (s, a) => roleAdapter.removeOne(a.roleId, s)),
  on(renameRoleSuccessAction, (s, {role}) =>
    roleAdapter.updateOne(
      {
        id: role.roleId,
        changes: {
          name: role.name,
        },
      },
      s
    )
  ),
  on(moveUpRoleSuccessAction, (s, a) =>
    roleAdapter.setAll(
      a.roles.map((r) => ({
        groupId: r.groupId,
        roleId: r.roleId,
        name: r.name,
        priority: r.priority,
      })),
      s
    )
  ),
  on(moveDownRoleSuccessAction, (s, a) =>
    roleAdapter.setAll(
      a.roles.map((r) => ({
        groupId: r.groupId,
        roleId: r.roleId,
        name: r.name,
        priority: r.priority,
      })),
      s
    )
  )
);
