import {GroupId, RoleId} from 'src/app/home/groups/store/types';
import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {createReducer, on} from '@ngrx/store';
import {
  getRolePermissionsAction,
  getRolePermissionsFailureAction,
  getRolePermissionsSuccessAction,
  updateRolePermissionsSuccessAction,
} from 'src/app/home/groups/store/roles/actions';

export interface RolePermission {
  id: string;
  groupId: GroupId;
  roleId: RoleId;
  name: string;
  code: string;
  isOn: boolean;
}

export const rolePermissionAdapter = createEntityAdapter<RolePermission>({
  selectId: (x) => x.id,
});

export interface RolePermissionsState extends EntityState<RolePermission> {
  isLoading: boolean;
}

export const rolePermissionReducer = createReducer(
  rolePermissionAdapter.getInitialState({isLoading: false}),
  on(getRolePermissionsAction, (s) => ({
    ...s,
    isLoading: true,
  })),
  on(getRolePermissionsSuccessAction, (s, {roles}) => ({
    ...rolePermissionAdapter.setAll(
      roles.map((r) => ({
        id: `${r.name}_${r.roleId}`,
        name: r.name,
        code: r.code,
        isOn: r.isOn,
        roleId: r.roleId,
        groupId: r.groupId,
      })),
      s
    ),
    isLoading: false,
  })),
  on(getRolePermissionsFailureAction, (s) => ({
    ...s,
    isLoading: false,
  })),
  on(updateRolePermissionsSuccessAction, (s, a) =>
    rolePermissionAdapter.upsertMany(
      a.permissions.map((p) => ({
        id: `${p.name}_${p.roleId}`,
        roleId: p.roleId,
        groupId: p.groupId,
        name: p.name,
        isOn: p.isOn,
        code: p.code,
      })),
      s
    )
  )
);
