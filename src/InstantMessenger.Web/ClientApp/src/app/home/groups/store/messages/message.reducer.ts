import {ChannelId, GroupId, UserId} from 'src/app/home/groups/store/types';
import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {createReducer, on} from '@ngrx/store';
import {
  getMessagesAction,
  getMessagesSuccessAction,
  sendMessageSuccessAction,
} from 'src/app/home/groups/store/messages/actions';

export type MessageId = string;

export interface Message {
  id: MessageId;
  groupId: GroupId;
  channelId: ChannelId;
  senderId: UserId;
  senderName: string;
  senderAvatar: string;
  content: string;
  createdAt: Date;
}

export interface MessagesState extends EntityState<Message> {
  isLoading: boolean;
}

export const messagesAdapter = createEntityAdapter<Message>({
  selectId: (x) => x.id,
});

export const messageReducer = createReducer(
  messagesAdapter.getInitialState({isLoading: false}),
  on(getMessagesAction, (s) => ({
    ...s,
    isLoading: true,
  })),
  on(getMessagesSuccessAction, (s, {messages}) => ({
    ...messagesAdapter.setAll(
      messages.map((m) => ({
        id: m.messageId,
        channelId: m.channelId,
        groupId: m.groupId,
        createdAt: new Date(m.createdAt),
        senderId: m.senderId,
        content: m.content,
        senderAvatar: m.senderAvatar,
        senderName: m.senderName,
      })),
      s
    ),
  })),
  on(sendMessageSuccessAction, (s, {message}) => ({
    ...messagesAdapter.addOne(
      {
        id: message.messageId,
        groupId: message.groupId,
        channelId: message.channelId,
        senderId: message.senderId,
        content: message.content,
        createdAt: new Date(message.createdAt),
        senderName: message.senderName,
        senderAvatar: message.senderAvatar,
      },
      s
    ),
  }))
);
