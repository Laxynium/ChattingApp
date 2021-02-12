import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {Action, createReducer, on} from '@ngrx/store';
import {InvitationsModal} from 'src/app/home/groups/components/group/invitations.modal';
import {
  ChannelDto,
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';
import {
  createChannelAction,
  createChannelFailureAction,
  createChannelSuccessAction,
  getChannelsAction,
  getChannelsFailureAction,
  getChannelsSuccessAction,
  loadCurrentChannelSuccessAction,
} from 'src/app/home/groups/store/channels/actions';
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
  channels: ChannelsModel;
  // members: Map<Guid, MemberModel>;
  // roles: Map<Guid, RoleModel>;
  // invitations: Map<Guid, InvitationModel>;
}

export interface ChannelsModel extends EntityState<ChannelModel> {
  isLoading: boolean;
  current: Guid;
}

export interface ChannelModel {
  id: Guid;
  name: string;
  // messages: Map<Guid, MessageModel>;
  // roleOverrides: Map<Guid, RoleOverride>;
  // memberOverrides: Map<Guid, MemberOverride>;
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

const channelsAdapter = createEntityAdapter<ChannelModel>({
  selectId: (x) => x.id,
  sortComparer: (x, y) => x.name.localeCompare(y.name),
});
const channelsState = channelsAdapter.getInitialState({
  isLoading: false,
  current: null,
});

function toModel(dto: GroupDto): GroupModel {
  return {
    id: dto.groupId,
    name: dto.name,
    createdAt: new Date(dto.createdAt),
    ownerId: dto.ownerId,
    channels: channelsState,
    // members: new Map<Guid, MemberModel>(),
    // roles: new Map<Guid, RoleModel>(),
  };
}

function toModel2(dto: ChannelDto): ChannelModel {
  return {
    id: dto.channelId,
    name: dto.channelName,
  };
}

const groupsReducer = createReducer(
  initState,
  on(getGroupsAction, (s) => ({...s, isLoading: true})),
  on(getGroupsSuccessAction, (s, a) => ({
    ...adapter.setAll(a.groups.map(toModel), s),
    isLoading: false,
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
    ...adapter.upsertOne(
      {
        id: a.group.groupId,
        name: a.group.name,
        ownerId: a.group.ownerId,
        createdAt: new Date(a.group.createdAt),
        channels: s.entities[a.group.groupId]?.channels ?? channelsState,
      },
      s
    ),
    current: a.group.groupId,
  })),
  on(loadCurrentGroupFailureAction, (s) => ({
    ...s,
  })),

  on(getChannelsAction, (s) => ({
    ...s,
    isLoading: true,
  })),
  on(getChannelsSuccessAction, (s, a) => ({
    ...adapter.upsertOne(
      {
        id: a.groupId,
        name: s.entities[a.groupId]?.name ?? '',
        ownerId: s.entities[a.groupId]?.ownerId ?? '',
        createdAt: s.entities[a.groupId]?.createdAt ?? new Date(null),
        channels: channelsAdapter.setAll(
          a.channels.map(toModel2),
          s.entities[a.groupId].channels
        ),
      },
      s
    ),
    isLoading: false,
  })),
  on(getChannelsFailureAction, (s) => ({
    ...s,
    isLoading: false,
  })),

  on(createChannelAction, (s) => ({
    ...s,
  })),
  on(createChannelSuccessAction, (s, a) => ({
    ...adapter.updateOne(
      {
        id: a.channel.groupId,
        changes: {
          channels: channelsAdapter.addOne(
            {
              id: a.channel.channelId,
              name: a.channel.channelName,
            },
            s.entities[a.channel.groupId].channels
          ),
        },
      },
      s
    ),
  })),
  on(createChannelFailureAction, (s) => ({
    ...s,
  })),

  on(loadCurrentChannelSuccessAction, (s, a) => ({
    ...adapter.updateOne(
      {
        id: s.current,
        changes: {
          channels: {...s.entities[s.current].channels, current: a.channelId},
        },
      },
      s
    ),
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

export const selectAllChannels = channelsAdapter.getSelectors().selectAll;
