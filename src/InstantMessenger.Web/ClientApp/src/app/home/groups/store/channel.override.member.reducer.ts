import {ChannelId, MemberId, PermissionOverrideType} from './types';
import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {createReducer, on} from '@ngrx/store';
import {
  getChannelMemberPermissionOverridesAction,
  getChannelMemberPermissionOverridesFailureAction,
  getChannelMemberPermissionOverridesSuccessAction,
  updateChannelMemberPermissionOverridesSuccessAction,
} from './channels/actions';

export interface MemberPermissionOverride {
  id: string;
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
  selectId: (x) => x.id,
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
        id: `${x.permission}_${a.memberUserId}_${a.channelId}`,
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
        id: `${x.permission}_${a.memberUserId}_${a.channelId}`,
        channelId: a.channelId,
        permission: x.permission,
        memberUserId: a.memberUserId,
        type: <PermissionOverrideType>(<unknown>x.type),
      })),
      s
    ),
  }))
);
