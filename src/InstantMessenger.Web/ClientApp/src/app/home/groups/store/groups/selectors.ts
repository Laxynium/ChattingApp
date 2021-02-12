import {faSdCard} from '@fortawesome/free-solid-svg-icons';
import {createFeatureSelector, createSelector} from '@ngrx/store';
import {
  GroupModel,
  GroupsModel,
  selectAllGroups,
} from 'src/app/home/groups/model';
import {
  ChannelDto,
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {AppStateInterface} from 'src/app/shared/types/appState.interface';

const featureSelector = createFeatureSelector<
  AppStateInterface,
  {
    groups: GroupsStateInterface;
    new: GroupsModel;
  }
>('groups');

export const groupsFeatureSelector = createSelector(
  featureSelector,
  (s) => s.groups
);

export const groupsSelectorNew = createSelector(featureSelector, (s) => s.new);

export const groupsSelector = createSelector(
  groupsSelectorNew,
  (s: GroupsModel) => selectAllGroups(s).map(toDto)
);

export const groupsLoadingSelector = createSelector(
  groupsSelectorNew,
  (s: GroupsModel): boolean => s.isLoading
);

export const currentGroupSelector = createSelector(
  groupsSelectorNew,
  (s: GroupsModel): GroupDto => toDto(s.entities[s.current])
);

function toDto(model: GroupModel): GroupDto {
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
