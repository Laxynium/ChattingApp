import {GroupId, MemberId, RoleId, UserId} from 'src/app/home/groups/store/types';
import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {createReducer, on} from '@ngrx/store';
import {
  addRoleToMemberSuccessAction,
  getMemberRolesAction,
  getMemberRolesFailureAction,
  getMemberRolesSuccessAction,
  removeRoleFromMemberSuccessAction,
} from 'src/app/home/groups/store/members/actions';

export interface MemberRole {
  id: string;
  memberId: MemberId;
  groupId: GroupId;
  userId: UserId;
  roleId: RoleId;
}

export const memberRoleAdapter = createEntityAdapter<MemberRole>({
  selectId: (x) => x.id,
});

export interface MemberRolesState extends EntityState<MemberRole> {
  isLoading: boolean;
}

export const memberRolesReducer = createReducer(
  memberRoleAdapter.getInitialState({isLoading: false}),
  on(getMemberRolesAction, (s) => ({...s, isLoading: true})),
  on(getMemberRolesSuccessAction, (s, a) => ({
    ...memberRoleAdapter.setAll(
      a.roles.map((r) => ({
        id: `${r.name}_${r.roleId}_${a.memberId}`,
        userId: a.userId,
        memberId: a.memberId,
        groupId: r.groupId,
        roleId: r.roleId,
        name: r.name,
        priority: r.priority,
      })),
      s
    ),
    isLoading: false,
  })),
  on(getMemberRolesFailureAction, (s) => ({...s, isLoading: false})),
  on(addRoleToMemberSuccessAction, (s, a) => ({
    ...memberRoleAdapter.upsertOne(
      {
        id: `${a.roleId}_${a.memberId}`,
        memberId: a.memberId,
        groupId: a.groupId,
        userId: a.userId,
        roleId: a.roleId,
      },
      s
    ),
  })),
  on(removeRoleFromMemberSuccessAction, (s, a) => ({
    ...memberRoleAdapter.removeOne(
      `${a.roleId}_${a.memberId}`,
      s
    ),
  }))
);
