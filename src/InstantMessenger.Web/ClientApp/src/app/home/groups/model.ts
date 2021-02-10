import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {Action, createReducer, on} from '@ngrx/store';
import {InvitationsModal} from 'src/app/home/groups/components/group/invitations.modal';
import {GroupDto} from 'src/app/home/groups/services/responses/group.dto';
import {
  createGroupAction,
  createGroupFailureAction,
  createGroupSuccessAction,
  getGroupsAction,
  getGroupsFailureAction,
  getGroupsSuccessAction,
  leaveGroupAction,
  leaveGroupFailureAction,
  leaveGroupSuccessAction,
  loadCurrentGroupAction,
  loadCurrentGroupFailureAction,
  loadCurrentGroupSuccessAction,
  removeGroupAction,
  removeGroupFailureAction,
  removeGroupSuccessAction,
  renameGroupSuccessAction,
} from 'src/app/home/groups/store/groups/actions';

type Guid = string;

export interface GroupsModel extends EntityState<GroupModel> {
  isLoading: false;
  current: Guid | null;
}

export interface GroupModel {
  id: Guid;
  name: string;
  ownerId: Guid;
  createdAt: Date;
  channels: Map<Guid, ChannelModel>;
  currentChannel?: Guid;
  members: Map<Guid, MemberModel>;
  roles: Map<Guid, RoleModel>;
  invitations: Map<Guid, InvitationModel>;
}

interface ChannelsModel extends EntityState<ChannelModel> {
  isLoading: false;
  current: Guid;
}

interface ChannelModel {
  id: Guid;
  name: string;
  messages: Map<Guid, MessageModel>;
  roleOverrides: Map<Guid, RoleOverride>;
  memberOverrides: Map<Guid, MemberOverride>;
}

interface MemberOverride {
  userId: Guid;
}

interface RoleOverride {
  id: Guid;
}

interface MessagesModel extends EntityState<MessageModel> {
  isLoading: false;
}

interface MessageModel {
  id: Guid;
  createdAt: Date;
  content: string;
  senderId: string;
}

interface MemberModel {}

interface RoleModel {}

interface InvitationModel {}

const adapter = createEntityAdapter<GroupModel>({
  selectId: (x) => x.id,
  sortComparer: (x, y) => x.name.localeCompare(y.name),
});
const initState = adapter.getInitialState({
  isLoading: false,
  current: null,
});

function toModel(dto: GroupDto): GroupModel {
  return {
    id: dto.groupId,
    name: dto.name,
    createdAt: new Date(dto.createdAt),
    ownerId: dto.ownerId,
    currentChannel: null,
    invitations: new Map<Guid, InvitationModel>(),
    channels: new Map<Guid, ChannelModel>(),
    members: new Map<Guid, MemberModel>(),
    roles: new Map<Guid, RoleModel>(),
  };
}

const groupsReducer = createReducer(
  initState,
  on(getGroupsAction, (s) => ({...s, isLoading: false})),
  on(getGroupsSuccessAction, (s, a) => ({
    ...adapter.setAll(a.groups.map(toModel), s),
  })),
  on(getGroupsFailureAction, (s) => ({
    ...s,
    isLoading: false,
  })),

  on(createGroupAction, (s) => ({
    ...s,
  })),
  on(createGroupSuccessAction, (s, a) => ({
    ...adapter.upsertOne(toModel(a.group), s),
  })),
  on(createGroupFailureAction, (s) => ({
    ...s,
  })),

  on(removeGroupAction, (s) => ({
    ...s,
  })),
  on(removeGroupSuccessAction, (s, a) => ({
    ...adapter.removeOne(a.groupId, s),
  })),
  on(removeGroupFailureAction, (s) => ({
    ...s,
  })),

  on(leaveGroupAction, (s) => ({
    ...s,
  })),
  on(leaveGroupSuccessAction, (s, a) => ({
    ...adapter.removeOne(a.groupId, s),
  })),
  on(leaveGroupFailureAction, (s) => ({
    ...s,
  })),

  on(renameGroupSuccessAction, (s, a) => ({
    ...adapter.updateOne(
      {
        id: a.group.groupId,
        changes: {
          name: a.group.name,
        },
      },
      s
    ),
  })),

  on(loadCurrentGroupAction, (s) => ({
    ...s,
  })),
  on(loadCurrentGroupSuccessAction, (s, a) => ({
    ...s,
    current: a.group.groupId,
  })),
  on(loadCurrentGroupFailureAction, (s) => ({
    ...s,
  }))
);

export function reducer(state: GroupsModel | undefined, action: Action) {
  return groupsReducer(state, action);
}

const {
  selectIds,
  selectEntities,
  selectAll,
  selectTotal,
} = adapter.getSelectors();

export const selectAllGroups = selectAll;
