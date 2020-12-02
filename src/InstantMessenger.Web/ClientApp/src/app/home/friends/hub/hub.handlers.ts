import {Store} from '@ngrx/store';
import {acceptInvitationSuccessAction} from 'src/app/home/friends/store/actions';
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
  //store.dispatch(acceptInvitationSuccessAction({id: '1'}));
  console.log('Handling friendship created', data);
};
const onFriendshipInvitationSent: HubMethod<FriendshipInvitationHubDto> = (
  store,
  data
) => {
  store.dispatch(acceptInvitationSuccessAction({id: '1'}));
};
const onFriendshipInvitationAccepted: HubMethod<FriendshipInvitationHubDto> = (
  store,
  data
) => {
  store.dispatch(acceptInvitationSuccessAction({id: '1'}));
};
const onFriendshipInvitationRejected: HubMethod<FriendshipInvitationHubDto> = (
  store,
  data
) => {
  store.dispatch(acceptInvitationSuccessAction({id: '1'}));
};
const onFriendshipInvitationCanceled: HubMethod<FriendshipInvitationHubDto> = (
  store,
  data
) => {
  store.dispatch(acceptInvitationSuccessAction({id: '1'}));
};

const hubProvider: HubHandlersProvider = () => ({
  onFriendshipCreated: onFriendshipCreated,
  onFriendshipInvitationSent: onFriendshipInvitationSent,
  onFriendshipInvitationAccepted: onFriendshipInvitationAccepted,
  onFriendshipInvitationRejected: onFriendshipInvitationRejected,
  onFriendshipInvitationCanceled: onFriendshipInvitationCanceled,
});

export const friendshipsHub: Hub = {
  hubName: 'friendships/hub',
  hubHandlersProvider: hubProvider,
};
