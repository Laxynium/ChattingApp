import {createSelector} from '@ngrx/store';
import {PermissionDto} from 'src/app/home/groups/store/types/permission';
import {RoleDto} from 'src/app/home/groups/store/types/role';
import {rolePermissionsStateSelector, rolesStateSelector} from '../selectors';
import {roleAdapter, RolesState} from '../role.redcuer';
import {
  rolePermissionAdapter,
  RolePermissionsState,
} from '../role.permission.reducer';

const {selectAll} = roleAdapter.getSelectors();
const rolePermissionSelectors = rolePermissionAdapter.getSelectors();
export const rolesSelector = createSelector(
  rolesStateSelector,
  (s: RolesState): RoleDto[] =>
    selectAll(s)
      .slice()
      .sort((a, b) => {
        if (a.priority == -1) return -1;
        return compare(a.priority, b.priority);
      })
      .reverse()
);

export const creatingRoleSelector = createSelector(
  rolesStateSelector,
  (s: RolesState): boolean => s.isBeingCreated
);

export const rolePermissionsSelector = createSelector(
  rolePermissionsStateSelector,
  (s: RolePermissionsState): PermissionDto[] =>
    rolePermissionSelectors.selectAll(s).map((x) => ({
      roleId: x.roleId,
      name: x.name,
      code: x.code,
      isOn: x.isOn,
      groupId: x.groupId,
    }))
);

export const rolePermissionsLoadingSelector = createSelector(
  rolePermissionsStateSelector,
  (s: RolePermissionsState): boolean => s.isLoading
);

function compare(a: number, b: number) {
  if (a < b) return -1;
  if (a == b) return 0;
  return 1;
}
