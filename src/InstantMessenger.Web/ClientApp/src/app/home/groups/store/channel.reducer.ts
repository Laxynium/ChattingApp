import {createEntityAdapter, EntityState} from "@ngrx/entity";
import {ChannelId, GroupId} from "./types";
import {createReducer, on} from "@ngrx/store";
import {
  createChannelSuccessAction,
  getChannelsAction,
  getChannelsFailureAction,
  getChannelsSuccessAction, loadCurrentChannelSuccessAction, removeChannelSuccessAction, renameChannelSuccessAction
} from "./channels/actions";

export interface Channel {
  id: ChannelId;
  groupId: GroupId;
  name: string;
}

export interface ChannelsState extends EntityState<Channel> {
  isLoading: boolean;
  currentChannel: ChannelId;
}

export const channelAdapter = createEntityAdapter<Channel>({
  selectId: (x) => x.id,
  sortComparer: (x, y) => x.name.localeCompare(y.name),
});

export const channelReducer = createReducer(
  channelAdapter.getInitialState({
    isLoading: false,
    currentChannel: null,
  }),
  on(createChannelSuccessAction, (s, {channel}) =>
    channelAdapter.addOne(
      {
        id: channel.channelId,
        groupId: channel.groupId,
        name: channel.channelName,
      },
      s
    )
  ),
  on(getChannelsAction, (s) => ({
    ...s,
    isLoading: true,
  })),
  on(getChannelsSuccessAction, (s, {groupId, channels}) => ({
    ...channelAdapter.setAll(
      channels.map((c) => ({
        id: c.channelId,
        name: c.channelName,
        groupId: c.groupId,
      })),
      s
    ),
    isLoading: false,
  })),
  on(getChannelsFailureAction, (s) => ({
    ...s,
    isLoading: true,
  })),
  on(renameChannelSuccessAction, (s, {channel}) =>
    channelAdapter.updateOne(
      {
        id: channel.channelId,
        changes: {
          name: channel.channelName,
        },
      },
      s
    )
  ),
  on(removeChannelSuccessAction, (s, {groupId, channelId}) =>
    channelAdapter.removeOne(channelId, s)
  ),
  on(loadCurrentChannelSuccessAction, (s, {channelId}) => ({
    ...s,
    currentChannel: channelId,
  }))
);
