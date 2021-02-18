import {createSelector} from '@ngrx/store';
import {
  memberRolesStateSelector,
  membersStateSelector,
  rolesStateSelector,
} from '../selectors';
import {
  Member,
  memberAdapter,
  MembersState,
} from 'src/app/home/groups/store/members/member.reducer';
import {
  memberRoleAdapter,
  MemberRolesState,
} from 'src/app/home/groups/store/members/member.role.reducer';
import {
  Role,
  roleAdapter,
  RolesState,
} from 'src/app/home/groups/store/roles/role.redcuer';
import {Set} from 'immutable';

const membersSelectors = memberAdapter.getSelectors();
const roleSelectors = roleAdapter.getSelectors();
export const membersSelector = createSelector(
  membersStateSelector,
  (s: MembersState): Member[] =>
    membersSelectors.selectAll(s).map((x) => ({
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
    memberRolesSelectors
      .selectAll(s)
      .filter((m) => roles.entities[m.roleId])
      .map((m) => roles.entities[m.roleId])
      .filter((r) => r.priority != -1)
);

export const availableRolesSelector = createSelector(
  memberRolesStateSelector,
  rolesStateSelector,
  (memberRoles: MemberRolesState, roles: RolesState): Role[] => {
    return Set(roleSelectors.selectAll(roles))
      .subtract(
        memberRolesSelectors
          .selectAll(memberRoles)
          .filter((m) => roles.entities[m.roleId])
          .map((m) => roles.entities[m.roleId])
      )
      .toArray();
  }
);
