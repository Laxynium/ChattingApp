import {createSelector} from '@ngrx/store';
import {memberRolesStateSelector, membersStateSelector, rolesStateSelector} from '../selectors';
import {
  Member,
  memberAdapter,
  MembersState,
} from 'src/app/home/groups/store/members/member.reducer';
import {
  memberRoleAdapter,
  MemberRolesState,
} from 'src/app/home/groups/store/members/member.role.reducer';
import {Role, RolesState} from "src/app/home/groups/store/roles/role.redcuer";

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
  rolesStateSelector,
  (s: MemberRolesState, roles: RolesState): Role[] =>
    memberRolesSelectors.selectAll(s).map(m=>roles[m.roleId]).filter((r) => r.priority != -1)
);
