import {createFeatureSelector, createSelector} from '@ngrx/store';
import {
  ChannelDto,
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
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
