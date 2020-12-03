import {Store} from '@ngrx/store';
import {
  acceptInvitationSuccessAction,
  getFriendsAction,
  getPendingInvitationsAction,
} from 'src/app/home/friends/store/actions';
import {
  Hub,
  HubHandlersProvider,
  HubMethod,
} from 'src/app/shared/hubs/hubHandlersProvider';

interface FriendshipHubDto {
  id: string;
  firstPerson: string;
  secondPerson: string;
  createdAt: string;
}
interface FriendshipInvitationHubDto {
  id: string;
  from: string;
  to: string;
  createdAt: string;
}

const onFriendshipCreated: HubMethod<FriendshipHubDto> = (store, data) => {
  store.dispatch(getFriendsAction());
};
const onFriendshipInvitationCreated: HubMethod<FriendshipInvitationHubDto> = (
  store,
  data
) => {
  store.dispatch(getPendingInvitationsAction());
};
const onFriendshipInvitationAccepted: HubMethod<FriendshipInvitationHubDto> = (
  store,
  data
) => {
  store.dispatch(getPendingInvitationsAction());
};
const onFriendshipInvitationRejected: HubMethod<FriendshipInvitationHubDto> = (
  store,
  data
) => {
  store.dispatch(getPendingInvitationsAction());
};
const onFriendshipInvitationCanceled: HubMethod<FriendshipInvitationHubDto> = (
  store,
  data
) => {
  store.dispatch(getPendingInvitationsAction());
};

const hubProvider: HubHandlersProvider = () => ({
  onFriendshipCreated: onFriendshipCreated,
  onFriendshipInvitationCreated: onFriendshipInvitationCreated,
  onFriendshipInvitationAccepted: onFriendshipInvitationAccepted,
  onFriendshipInvitationRejected: onFriendshipInvitationRejected,
  onFriendshipInvitationCanceled: onFriendshipInvitationCanceled,
});

export const friendshipsHub: Hub = {
  hubName: 'friendships/hub',
  hubHandlersProvider: hubProvider,
};
