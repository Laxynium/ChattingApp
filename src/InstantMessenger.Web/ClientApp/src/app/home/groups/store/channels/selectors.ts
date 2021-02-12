import {createSelector} from '@ngrx/store';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {
  groupsFeatureSelector,
  groupsSelectorNew,
} from 'src/app/home/groups/store/groups/selectors';
import {PermissionOverrideDto} from 'src/app/home/groups/store/types/role-permission-override';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';
import {
  ChannelModel,
  GroupsModel,
  selectAllChannels,
} from 'src/app/home/groups/model';

export const overridesSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): PermissionOverrideDto[] => s.overrides
);

export const overridesLoadingSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.overridesLoading
);

export const channelsSelector = createSelector(
  groupsSelectorNew,
  (s: GroupsModel): ChannelDto[] => {
    const currentGroup = s.entities[s.current];
    if (currentGroup) {
      return selectAllChannels(currentGroup.channels).map((x) =>
        toDto(s.current, x)
      );
    } else {
      return [];
    }
  }
);

export const currentChannelSelector = createSelector(
  groupsSelectorNew,
  (s: GroupsModel): ChannelDto => {
    const currentGroup = s.entities[s.current];
    const currentChannel = currentGroup.channels[currentGroup.channels.current];
    return toDto(s.current, currentChannel);
  }
);

function toDto(groupId: string, model: ChannelModel): ChannelDto {
  return {
    groupId: groupId,
    channelId: model.id,
    channelName: model.name,
  };
}
