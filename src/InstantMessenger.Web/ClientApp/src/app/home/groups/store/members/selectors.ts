import {createSelector} from '@ngrx/store';
import {RoleDto} from 'src/app/home/groups/store/types/role';
import {memberRolesStateSelector, membersStateSelector} from '../selectors';
import {
  Member,
  memberAdapter,
  MembersState,
} from 'src/app/home/groups/store/members/member.reducer';
import {
  memberRoleAdapter,
  MemberRolesState,
} from 'src/app/home/groups/store/members/member.role.reducer';

const membersSelectors = memberAdapter.getSelectors();
export const membersSelector = createSelector(
  membersStateSelector,
  (s: MembersState): Member[] =>
    membersSelectors
      .selectAll(s)
      .map((x) => ({
        ...x,
        avatar: x.avatar ? x.avatar : 'assets/profile-placeholder.png',
      }))
);
export const membersLoadingSelector = createSelector(
  membersStateSelector,
  (s: MembersState): boolean => s.isLoading
);

const memberRolesSelectors = memberRoleAdapter.getSelectors();
export const memberRolesSelector = createSelector(
  memberRolesStateSelector,
  (s: MemberRolesState): RoleDto[] =>
    memberRolesSelectors.selectAll(s).filter((r) => r.priority != -1)
);
