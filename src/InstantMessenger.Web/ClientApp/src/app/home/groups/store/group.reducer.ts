import {createReducer, on} from "@ngrx/store";
import {
  createGroupSuccessAction,
  getGroupsAction,
  getGroupsFailureAction,
  getGroupsSuccessAction,
  leaveGroupSuccessAction,
  loadCurrentGroupSuccessAction,
  removeGroupSuccessAction,
  renameGroupSuccessAction
} from "./groups/actions";
import {createEntityAdapter, EntityState} from "@ngrx/entity";
import {GroupId, UserId} from "./types";


export interface Group {
  id: GroupId;
  name: string;
  createdAt: string;
  ownerId: UserId;
}
export const groupAdapter = createEntityAdapter<Group>({
  selectId: (x) => x.id,
  sortComparer: (x, y) => x.name.localeCompare(y.name),
});
export interface GroupsState extends EntityState<Group> {
  isLoading: boolean;
  currentGroup: GroupId;
}

export const groupReducer = createReducer(
  groupAdapter.getInitialState({isLoading: false, currentGroup: null}),
  on(createGroupSuccessAction, (s, {group}) =>
    groupAdapter.addOne(
      {
        id: group.groupId,
        name: group.name,
        createdAt: group.createdAt,
        ownerId: group.ownerId,
      },
      s
    )
  ),
  on(getGroupsAction, (s) => ({
    ...s,
    isLoading: true,
  })),
  on(getGroupsSuccessAction, (s, {groups}) => ({
    ...groupAdapter.setAll(
      groups.map((g) => ({
        id: g.groupId,
        name: g.name,
        createdAt: g.createdAt,
        ownerId: g.ownerId,
      })),
      s
    ),
    isLoading: false,
  })),
  on(getGroupsFailureAction, (s) => ({
    ...s,
    isLoading: false,
  })),
  on(renameGroupSuccessAction, (s, {group}) =>
    groupAdapter.updateOne(
      {
        id: group.groupId,
        changes: {
          name: group.name,
        },
      },
      s
    )
  ),
  on(removeGroupSuccessAction, (s, {groupId}) =>
    groupAdapter.removeOne(groupId, s)
  ),
  on(leaveGroupSuccessAction, (s, {groupId}) =>
    groupAdapter.removeOne(groupId, s)
  ),
  on(loadCurrentGroupSuccessAction, (s, {group}) => ({
    ...groupAdapter.upsertOne(
      {
        id: group.groupId,
        name: group.name,
        createdAt: group.createdAt,
        ownerId: group.createdAt,
      },
      s
    ),
    currentGroup: group.groupId,
  }))
);
