import {
  getFriendsAction,
  getPendingInvitationsAction,
  removeFriendSuccessAction,
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

const onFriendshipCreated: HubMethod<FriendshipHubDto> = (store) => {
  store.dispatch(getFriendsAction());
};
const onFriendshipInvitationCreated: HubMethod<FriendshipInvitationHubDto> = (
  store
) => {
  store.dispatch(getPendingInvitationsAction());
};
const onFriendshipInvitationAccepted: HubMethod<FriendshipInvitationHubDto> = (
  store
) => {
  store.dispatch(getPendingInvitationsAction());
};
const onFriendshipInvitationRejected: HubMethod<FriendshipInvitationHubDto> = (
  store
) => {
  store.dispatch(getPendingInvitationsAction());
};
const onFriendshipInvitationCanceled: HubMethod<FriendshipInvitationHubDto> = (
  store
) => {
  store.dispatch(getPendingInvitationsAction());
};

const onFriendshipRemoved: HubMethod<FriendshipHubDto> = (store, data) => {
  store.dispatch(removeFriendSuccessAction({friendshipId: data.id}));
};

const hubProvider: HubHandlersProvider = () => ({
  onFriendshipCreated: onFriendshipCreated,
  onFriendshipInvitationCreated: onFriendshipInvitationCreated,
  onFriendshipInvitationAccepted: onFriendshipInvitationAccepted,
  onFriendshipInvitationRejected: onFriendshipInvitationRejected,
  onFriendshipInvitationCanceled: onFriendshipInvitationCanceled,
  onFriendshipRemoved: onFriendshipRemoved,
});

export const friendshipsHub: Hub = {
  hubName: 'friendships/hub',
  hubHandlersProvider: hubProvider,
};
