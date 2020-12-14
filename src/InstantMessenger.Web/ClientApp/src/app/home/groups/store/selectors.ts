import {createFeatureSelector, createSelector} from '@ngrx/store';
import {
  ChannelDto,
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {InvitationDto} from 'src/app/home/groups/store/types/invitation';
import {AppStateInterface} from 'src/app/shared/types/appState.interface';

export const groupsFeatureSelector = createFeatureSelector<
  AppStateInterface,
  GroupsStateInterface
>('groups');

export const groupsSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): GroupDto[] => s.groups
);

export const groupsLoadingSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.groupsLoading
);

export const currentGroupSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): GroupDto => s.currentGroup
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
export const currentInvitationSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): {code: string; isBeingGenerated: boolean} => ({
    code: s.generatedInvitation.code,
    isBeingGenerated: s.generatedInvitation.isBeingGenerated,
  })
);

export const invitationsSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): InvitationDto[] => s.invitations
);

export const isInvitationBeingGeneratedSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): boolean => s.generatedInvitation.isBeingGenerated
);
