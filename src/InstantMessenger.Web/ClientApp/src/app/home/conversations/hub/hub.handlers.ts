import {HubMessage} from '@microsoft/signalr';
import {
  markAsReadActionSuccessAction,
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

interface MessageReadHubDto {
  messageId: string;
  conversationId: string;
  senderId: string;
  receiverId: string;
  readAt: string;
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

const onMessageRead: HubMethod<MessageReadHubDto> = (store, data) => {
  store.dispatch(
    markAsReadActionSuccessAction({
      marked: [
        {
          messageId: data.messageId,
          readAt: data.conversationId,
        },
      ],
    })
  );
};

const hubProvider: HubHandlersProvider = () => ({
  onMessageCreated: onMessageCreated,
  onMessageRead: onMessageRead,
});

export const conversationsHub: Hub = {
  hubName: 'privateMessages/hub',
  hubHandlersProvider: hubProvider,
};
