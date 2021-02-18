import {
  ChannelId,
  MemberId,
  PermissionOverrideType,
} from 'src/app/home/groups/store/types';
import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {createReducer, on} from '@ngrx/store';
import {
  getChannelMemberPermissionOverridesAction,
  getChannelMemberPermissionOverridesFailureAction,
  getChannelMemberPermissionOverridesSuccessAction,
  updateChannelMemberPermissionOverridesSuccessAction,
} from 'src/app/home/groups/store/channels/actions';

export interface MemberPermissionOverride {
  channelId: ChannelId;
  memberUserId: MemberId;
  permission: string;
  type: PermissionOverrideType;
}

export interface MemberPermissionOverridesState
  extends EntityState<MemberPermissionOverride> {
  isLoading: boolean;
}

export const adapter = createEntityAdapter<MemberPermissionOverride>({
  selectId: (x) => `${x.permission}_${x.memberUserId}_${x.channelId}`,
});

export const reducer = createReducer(
  adapter.getInitialState({isLoading: false}),
  on(getChannelMemberPermissionOverridesAction, (s) => ({
    ...s,
    isLoading: true,
  })),
  on(getChannelMemberPermissionOverridesSuccessAction, (s, a) => ({
    ...adapter.setAll(
      a.overrides.map((x) => ({
        channelId: a.channelId,
        permission: x.permission,
        memberUserId: a.memberUserId,
        type: <PermissionOverrideType>(<unknown>x.type),
      })),
      s
    ),
    isLoading: false,
  })),
  on(getChannelMemberPermissionOverridesFailureAction, (s) => ({
    ...s,
    isLoading: false,
  })),
  on(updateChannelMemberPermissionOverridesSuccessAction, (s, a) => ({
    ...adapter.upsertMany(
      a.overrides.map((x) => ({
        channelId: a.channelId,
        permission: x.permission,
        memberUserId: a.memberUserId,
        type: <PermissionOverrideType>(<unknown>x.type),
      })),
      s
    ),
  }))
);
