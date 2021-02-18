import {createSelector} from '@ngrx/store';

import {GroupDto} from 'src/app/home/groups/services/responses/group.dto';
import {
  Group,
  groupAdapter,
  GroupsState,
} from 'src/app/home/groups/store/groups/group.reducer';
import {groupsStateSelector} from 'src/app/home/groups/store/selectors';

const {selectAll} = groupAdapter.getSelectors();

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
