import {createSelector} from '@ngrx/store';
import {
  rolePermissionsStateSelector,
  rolesStateSelector,
} from 'src/app/home/groups/store/selectors';
import {
  Role,
  roleAdapter,
  RolesState,
} from 'src/app/home/groups/store/roles/role.redcuer';
import {
  RolePermission,
  rolePermissionAdapter,
  RolePermissionsState,
} from '../roles/role.permission.reducer';

const {selectAll} = roleAdapter.getSelectors();
const rolePermissionSelectors = rolePermissionAdapter.getSelectors();
export const rolesSelector = createSelector(
  rolesStateSelector,
  (s: RolesState): Role[] =>
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
  (s: RolePermissionsState): RolePermission[] =>
    rolePermissionSelectors.selectAll(s)
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
