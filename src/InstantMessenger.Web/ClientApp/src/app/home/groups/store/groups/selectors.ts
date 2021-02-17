import {createFeatureSelector, createSelector} from '@ngrx/store';

import {
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {AppStateInterface} from 'src/app/shared/types/appState.interface';
import {GroupsModuleState} from "../reducers2";
import {Group, groupAdapter, GroupsState} from "../group.reducer";
import {groupsStateSelector} from "../selectors";

const {selectAll} = groupAdapter.getSelectors()

const featureSelector = createFeatureSelector<
  AppStateInterface,
  GroupsModuleState & {groupsOld:GroupsStateInterface}
>('groups');

export const groupsFeatureSelector = createSelector(
  featureSelector,
  (s) => s.groupsOld
);

export const groupsSelector = createSelector(
  groupsStateSelector,
  (s: GroupsState) => selectAll(s).map(toDto)
);

export const groupsLoadingSelector = createSelector(
  groupsStateSelector,
  (s: GroupsState): boolean => s.isLoading
);

export const currentGroupSelector = createSelector(
  groupsStateSelector,
  (s: GroupsState): GroupDto => toDto(s.entities[s.currentGroup])
);

function toDto(model: Group): GroupDto {
  if (model)
    return {
      groupId: model.id,
      name: model.name,
      createdAt: model.createdAt.toString(),
      ownerId: model.ownerId,
    };
  return {
    groupId: '',
    name: '',
    createdAt: '',
    ownerId: '',
  };
}
