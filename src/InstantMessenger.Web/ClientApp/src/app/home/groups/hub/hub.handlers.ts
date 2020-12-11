import {
  generateInvitationSuccessAction,
  getChannelsAction,
  getInvitationsAction,
  removeGroupSuccessAction,
} from 'src/app/home/groups/store/actions';
import {
  Hub,
  HubHandlersProvider,
  HubMethod,
} from 'src/app/shared/hubs/hubHandlersProvider';

interface GroupDto {
  groupId: string;
  groupName: string;
}
interface InvitationDto {
  groupId: string;
  invitationId: string;
  code: string;
}
interface ChannelDto {
  groupId: string;
  channelId: string;
  channelName: string;
}
const onGroupRemoved: HubMethod<GroupDto> = (store, data) => {
  store.dispatch(removeGroupSuccessAction({groupId: data.groupId}));
};
const onInvitationCreated: HubMethod<InvitationDto> = (store, data) => {
  store.dispatch(getInvitationsAction({groupId: data.groupId}));
};
const onInvitationRevoked: HubMethod<InvitationDto> = (store, data) => {
  store.dispatch(getInvitationsAction({groupId: data.groupId}));
};
const onChannelCreated: HubMethod<ChannelDto> = (store, data) => {
  store.dispatch(getChannelsAction({groupId: data.groupId}));
};
const onChannelRemoved: HubMethod<ChannelDto> = (store, data) => {
  store.dispatch(getChannelsAction({groupId: data.groupId}));
};
const hubProvider: HubHandlersProvider = () => ({
  onGroupRemoved: onGroupRemoved,
  onInvitationCreated: onInvitationCreated,
  onInvitationRevoked: onInvitationRevoked,
  onChannelCreated: onChannelCreated,
  onChannelRemoved: onChannelRemoved,
});

export const groupsHub: Hub = {
  hubName: 'groups/hub',
  hubHandlersProvider: hubProvider,
};
