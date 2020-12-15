import {createSelector} from '@ngrx/store';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {groupsFeatureSelector} from 'src/app/home/groups/store/selectors';
import {PermissionOverrideDto} from 'src/app/home/groups/store/types/role-permission-override';

export const overridesSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): PermissionOverrideDto[] => s.overrides
);

export const overridesLoadingSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.overridesLoading
);
