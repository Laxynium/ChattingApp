import {createSelector} from '@ngrx/store';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {groupsFeatureSelector} from 'src/app/home/groups/store/groups/selectors';
import {PermissionDto} from 'src/app/home/groups/store/types/permission';
import {RoleDto} from 'src/app/home/groups/store/types/role';

export const rolesSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): RoleDto[] =>
    s.roles
      .toList()
      .slice()
      .sort((a, b) => {
        if (a.priority == -1) return -1;
        return compare(a.priority, b.priority);
      })
      .reverse()
      .toArray()
);

export const creatingRoleSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.creatingRole
);

export const rolePermissionsSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): PermissionDto[] => s.rolePermissions
);

export const rolePermissionsLoadingSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.rolePermissionsLoading
);

function compare(a: number, b: number) {
  if (a < b) return -1;
  if (a == b) return 0;
  return 1;
}
