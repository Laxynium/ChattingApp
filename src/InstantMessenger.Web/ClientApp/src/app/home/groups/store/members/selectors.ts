import {createSelector} from '@ngrx/store';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {groupsFeatureSelector} from 'src/app/home/groups/store/selectors';
import {MemberDto} from 'src/app/home/groups/store/types/member';
import {RoleDto} from 'src/app/home/groups/store/types/role';

export const membersSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): MemberDto[] =>
    s.members.slice().map((m) => ({
      ...m,
      avatar: m.avatar ? m.avatar : 'assets/profile-placeholder.png',
    }))
);

export const membersLoadingSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.membersLoading
);

export const memberRolesSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): RoleDto[] =>
    s.memberRoles.filter((r) => r.priority != -1)
);
