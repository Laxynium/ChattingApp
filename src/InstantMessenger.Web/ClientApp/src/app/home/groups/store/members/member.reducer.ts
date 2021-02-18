import {GroupId, MemberId, UserId} from 'src/app/home/groups/store/types';
import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {createReducer, on} from '@ngrx/store';
import {
  getMembersAction,
  getMembersFailureAction,
  getMembersSuccessAction,
  kickMemberSuccessAction,
} from 'src/app/home/groups/store/members/actions';

export interface Member {
  memberId: MemberId;
  groupId: GroupId;
  userId: UserId;
  name: string;
  avatar: string;
  isOwner: boolean;
  createdAt: Date;
}

export const memberAdapter = createEntityAdapter<Member>({
  selectId: (x) => x.memberId,
});

export interface MembersState extends EntityState<Member> {
  isLoading: boolean;
}

export const membersReducer = createReducer(
  memberAdapter.getInitialState({isLoading: false}),
  on(getMembersAction, (s) => ({...s, isLoading: true})),
  on(getMembersSuccessAction, (s, {members}) => ({
    ...memberAdapter.setAll(
      members.map((m) => ({
        memberId: `${m.groupId}_${m.userId}`,
        groupId: m.groupId,
        userId: m.userId,
        avatar: m.avatar,
        name: m.name,
        createdAt: new Date(m.createdAt),
        isOwner: m.isOwner,
      })),
      s
    ),
    isLoading: false,
  })),
  on(getMembersFailureAction, (s) => ({...s, isLoading: false})),
  on(kickMemberSuccessAction, (s, a) => ({
    ...memberAdapter.removeOne(a.userId, s),
  }))
);
