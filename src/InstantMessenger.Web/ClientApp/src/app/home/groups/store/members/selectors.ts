import {createSelector} from '@ngrx/store';
import {MemberDto} from 'src/app/home/groups/store/types/member';
import {RoleDto} from 'src/app/home/groups/store/types/role';
import {memberRolesStateSelector, membersStateSelector} from '../selectors';
import {memberAdapter, MembersState} from 'src/app/home/groups/store/members/member.reducer';
import {memberRoleAdapter, MemberRolesState} from 'src/app/home/groups/store/members/member.role.reducer';

const membersSelectors = memberAdapter.getSelectors();
export const membersSelector = createSelector(
  membersStateSelector,
  (s: MembersState): MemberDto[] =>
    membersSelectors.selectAll(s).map((x) => ({
      memberId: x.id,
      userId: x.userId,
      isOwner: x.isOwner,
      groupId: x.groupId,
      name: x.name,
      createdAt: x.createdAt.toString(),
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
