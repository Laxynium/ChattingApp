import {Action, createReducer, on} from '@ngrx/store';
import {
  ChannelDto,
  GroupDto,
} from 'src/app/home/groups/services/responses/group.dto';
import {
  changeCurrentGroupAction,
  changeCurrentGroupFailureAction,
  changeCurrentGroupSuccessAction,
  createChannelAction,
  createChannelFailureAction,
  createChannelSuccessAction,
  createGroupAction,
  createGroupFailureAction,
  createGroupSuccessAction,
  getChannelsAction,
  getChannelsFailureAction,
  getChannelsSuccessAction,
  getGroupsAction,
  getGroupsFailureAction,
  getGroupsSuccessAction,
  joinGroupAction,
  joinGroupFailureAction,
  joinGroupSuccessAction,
  removeChannelAction,
  removeChannelFailureAction,
  removeChannelSuccessAction,
  removeGroupAction,
  removeGroupFailureAction,
  removeGroupSuccessAction,
} from 'src/app/home/groups/store/actions';
import {
  CurrentGroup,
  EmptyCurrentGroup,
  ICurrentGroup,
} from 'src/app/home/groups/store/types/currentGroup';

export interface GroupsStateInterface {
  groups: GroupDto[];
  groupsLoading: boolean;
  currentGroup: ICurrentGroup;
  channels: ChannelDto[];
}

const initialState: GroupsStateInterface = {
  groups: [],
  groupsLoading: false,
  currentGroup: new EmptyCurrentGroup(),
  channels: [],
};

const groupsReducer = createReducer(
  initialState,
  on(getGroupsAction, (s, a) => ({
    ...s,
    groupsLoading: true,
  })),
  on(getGroupsSuccessAction, (s, a) => ({
    ...s,
    groups: a.groups,
    groupsLoading: false,
  })),
  on(getGroupsFailureAction, (s, a) => ({
    ...s,
    groupsLoading: false,
  })),

  on(getChannelsAction, (s, a) => ({
    ...s,
  })),
  on(getChannelsSuccessAction, (s, a) => ({
    ...s,
    channels: [...a.channels],
  })),
  on(getChannelsFailureAction, (s, a) => ({
    ...s,
  })),

  on(createGroupAction, (s, a) => ({
    ...s,
  })),
  on(createGroupSuccessAction, (s, a) => ({
    ...s,
    groups: [...s.groups, a.group],
  })),
  on(createGroupFailureAction, (s, a) => ({
    ...s,
  })),

  on(removeGroupAction, (s, a) => ({
    ...s,
  })),
  on(removeGroupSuccessAction, (s, a) => ({
    ...s,
    groups: [...s.groups.filter((g) => g.groupId != a.groupId)],
  })),
  on(removeGroupFailureAction, (s, a) => ({
    ...s,
  })),

  on(joinGroupAction, (s, a) => ({
    ...s,
  })),
  on(joinGroupSuccessAction, (s, a) => ({
    ...s,
  })),
  on(joinGroupFailureAction, (s, a) => ({
    ...s,
  })),

  on(changeCurrentGroupAction, (s, a) => ({
    ...s,
  })),
  on(changeCurrentGroupSuccessAction, (s, a) => ({
    ...s,
    currentGroup: new CurrentGroup(a.groupId),
  })),
  on(changeCurrentGroupFailureAction, (s, a) => ({
    ...s,
  })),

  on(createChannelAction, (s, a) => ({
    ...s,
  })),
  on(createChannelSuccessAction, (s, a) => ({
    ...s,
    channels: [...s.channels, a.channel],
  })),
  on(createChannelFailureAction, (s, a) => ({
    ...s,
  })),

  on(removeChannelAction, (s, a) => ({
    ...s,
  })),
  on(removeChannelSuccessAction, (s, a) => ({
    ...s,
    channels: [...s.channels.filter((c) => c.channelId != a.channelId)],
  })),
  on(removeChannelFailureAction, (s, a) => ({
    ...s,
  }))
);
export function reducers(state: GroupsStateInterface, action: Action) {
  return groupsReducer(state, action);
}
