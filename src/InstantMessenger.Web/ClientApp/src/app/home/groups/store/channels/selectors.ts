import {createSelector} from '@ngrx/store';
import {
  PermissionOverrideDto,
  PermissionOverrideTypeDto,
} from 'src/app/home/groups/store/types/role-permission-override';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {Channel, channelAdapter, ChannelsState} from '../channel.reducer';
import {
  channelsStateSelector,
  currentGroupSelector,
  memberOverridesStateSelector,
  roleOverridesStateSelector,
} from '../selectors';
import {Group} from '../group.reducer';
import {
  adapter as roleOverridesAdapter,
  RolePermissionOverridesState,
} from '../channel.override.role.reducer';
import {
  adapter as memberOverridesAdapter,
  MemberPermissionOverridesState,
} from '../channel.override.member.reducer';

const roleOverridesSelectors = roleOverridesAdapter.getSelectors();

export const roleOverridesSelector = createSelector(
  roleOverridesStateSelector,
  (s: RolePermissionOverridesState): PermissionOverrideDto[] =>
    roleOverridesSelectors.selectAll(s).map(
      (x) =>
        <PermissionOverrideDto>{
          permission: x.permission,
          type: <PermissionOverrideTypeDto>(<unknown>x.type),
        }
    )
);
export const roleOverridesLoadingSelector = createSelector(
  roleOverridesStateSelector,
  (s: RolePermissionOverridesState): boolean => s.isLoading
);

const memberOverridesSelectors = memberOverridesAdapter.getSelectors();

export const memberOverridesSelector = createSelector(
  memberOverridesStateSelector,
  (s: MemberPermissionOverridesState): PermissionOverrideDto[] =>
    memberOverridesSelectors.selectAll(s).map(
      (x) =>
        <PermissionOverrideDto>{
          permission: x.permission,
          type: <PermissionOverrideTypeDto>(<unknown>x.type),
        }
    )
);

export const memberOverridesLoadingSelector = createSelector(
  memberOverridesStateSelector,
  (s: MemberPermissionOverridesState): boolean => s.isLoading
);

const channelSelectors = channelAdapter.getSelectors();

export const channelsSelector = createSelector(
  currentGroupSelector,
  channelsStateSelector,
  (group: Group, channels: ChannelsState): ChannelDto[] => {
    if (!group) {
      return [];
    }
    return channelSelectors
      .selectAll(channels)
      .filter((c) => c.groupId == group.id)
      .map((x) => toDto(x));
  }
);

export const currentChannelSelector = createSelector(
  channelsStateSelector,
  (s: ChannelsState): ChannelDto => {
    const currentChannel = s.entities[s.currentChannel];
    if (!currentChannel) return null;
    return toDto(currentChannel);
  }
);

function toDto(model: Channel): ChannelDto {
  return {
    groupId: model.groupId,
    channelId: model.id,
    channelName: model.name,
  };
}
