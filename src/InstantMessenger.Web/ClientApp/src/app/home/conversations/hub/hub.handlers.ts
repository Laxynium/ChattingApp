import {
  receiveMessageAction,
  receiveMessageSuccessAction,
} from 'src/app/home/conversations/store/actions';
import {
  Hub,
  HubHandlersProvider,
  HubMethod,
} from 'src/app/shared/hubs/hubHandlersProvider';

interface MessageHubDto {
  messageId: string;
  conversationId: string;
  senderId: string;
  receiverId: string;
  content: string;
  createdAt: string;
}

const onMessageCreated: HubMethod<MessageHubDto> = (store, data) => {
  store.dispatch(
    receiveMessageSuccessAction({
      message: {
        id: data.messageId,
        body: data.content,
        conversationId: data.conversationId,
        createdAt: data.createdAt,
        fromUserId: data.senderId,
        toUserId: data.receiverId,
        readAt: null,
      },
    })
  );
};

const hubProvider: HubHandlersProvider = () => ({
  onMessageCreated: onMessageCreated,
});

export const conversationsHub: Hub = {
  hubName: 'privateMessages/hub',
  hubHandlersProvider: hubProvider,
};
