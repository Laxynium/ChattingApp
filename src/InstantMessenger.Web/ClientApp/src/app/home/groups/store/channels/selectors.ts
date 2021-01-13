import {createSelector} from '@ngrx/store';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {groupsFeatureSelector} from 'src/app/home/groups/store/groups/selectors';
import {PermissionOverrideDto} from 'src/app/home/groups/store/types/role-permission-override';
import {ChannelDto} from 'src/app/home/groups/services/responses/group.dto';

export const overridesSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): PermissionOverrideDto[] => s.overrides
);

export const overridesLoadingSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.overridesLoading
);

export const channelsSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): ChannelDto[] =>
    s.channels
      .toList()
      .sortBy((x) => x.channelName)
      .toArray()
);

export const currentChannelSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): Object => s.currentChannel
);
