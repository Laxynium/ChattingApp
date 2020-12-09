import {createSelector} from '@ngrx/store';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {groupsFeatureSelector} from 'src/app/home/groups/store/selectors';
import {RoleDto} from 'src/app/home/groups/store/types/role';

export const rolesSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): RoleDto[] =>
    s.roles
      .slice()
      .sort((a, b) => {
        if (a.priority == -1) return -1;
        return compare(a.priority, b.priority);
      })
      .reverse()
);

export const creatingRoleSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.creatingRole
);

function compare(a: number, b: number) {
  if (a < b) return -1;
  if (a == b) return 0;
  return 1;
}
