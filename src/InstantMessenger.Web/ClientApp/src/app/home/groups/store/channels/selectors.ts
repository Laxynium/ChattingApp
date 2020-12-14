import {createSelector} from '@ngrx/store';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {groupsFeatureSelector} from 'src/app/home/groups/store/selectors';
import {PermissionOverrideDto} from 'src/app/home/groups/store/types/role-permission-override';

export const roleOverridesSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): PermissionOverrideDto[] => s.roleOverrides
);

export const roleOverridesLoadingSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.roleOverridesLoading
);
