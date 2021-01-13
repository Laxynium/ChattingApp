import {createAction, props} from '@ngrx/store';
import {ConversationResponseInterface} from 'src/app/home/conversations/store/types/conversation.response';
import {
  ConversationMessageInterface,
  LatestConversationInterface,
} from 'src/app/home/conversations/store/types/Conversation.interface';

export enum ActionTypes {
  GET_LATEST_CONVERSATIONS = '[Conversations] Get latest conversations',
  GET_LATEST_CONVERSATIONS_SUCCESS = '[Conversations] Get latest conversations success',
  GET_LATEST_CONVERSATIONS_FAILURE = '[Conversations] Get latest conversations failure',

  CHANGE_CONVERSATION = '[Conversations] Change conversation',
  CHANGE_CONVERSATION_SUCCESS = '[Conversations] Change conversation success',
  CHANGE_CONVERSATION_FAILURE = '[Conversations] Change conversation failure',

  SEND_MESSAGE = '[Conversations] Send message',
  SEND_MESSAGE_SUCCESS = '[Conversations] Send message success',
  SEND_MESSAGE_FAILURE = '[Conversations] Send message failure',

  RECEIVE_MESSAGE = '[Conversations] Receive message',
  RECEIVE_MESSAGE_SUCCESS = '[Conversations] Receive message success',
  RECEIVE_MESSAGE_FAILURE = '[Conversations] Receive message failure',

  MARK_AS_READ = '[Conversations] Mark as read',
  MARK_AS_READ_SUCCESS = '[Conversations] Mark as read success',
  MARK_AS_READ_FAILURE = '[Conversations] Mark as read failure',

  CONVERSATION_REMOVED = '[Conversations] Conversation removed',
}

export const getLatestConversationsAction = createAction(
  ActionTypes.GET_LATEST_CONVERSATIONS,
  props<{count: number}>()
);

export const getLatestConversationsSuccessAction = createAction(
  ActionTypes.GET_LATEST_CONVERSATIONS_SUCCESS,
  props<{conversations: LatestConversationInterface[]}>()
);

export const getLatestConversationsFailureAction = createAction(
  ActionTypes.GET_LATEST_CONVERSATIONS_FAILURE
);

export const changeConversationAction = createAction(
  ActionTypes.CHANGE_CONVERSATION,
  props<{conversationId: string}>()
);
export const changeConversationSuccessAction = createAction(
  ActionTypes.CHANGE_CONVERSATION_SUCCESS,
  props<{conversation: ConversationResponseInterface}>()
);
export const changeConversationFailureAction = createAction(
  ActionTypes.CHANGE_CONVERSATION_FAILURE
);
export const sendMessageAction = createAction(
  ActionTypes.SEND_MESSAGE,
  props<{conversationId: string; content: string}>()
);
export const sendMessageSuccessAction = createAction(
  ActionTypes.SEND_MESSAGE_SUCCESS,
  props<{conversationId: string; message: ConversationMessageInterface}>()
);
export const sendMessageFailureAction = createAction(
  ActionTypes.SEND_MESSAGE_FAILURE
);
export const receiveMessageAction = createAction(
  ActionTypes.RECEIVE_MESSAGE,
  props<{message: ConversationMessageInterface}>()
);
export const receiveMessageSuccessAction = createAction(
  ActionTypes.RECEIVE_MESSAGE_SUCCESS,
  props<{message: ConversationMessageInterface}>()
);
export const receiveMessageFailureAction = createAction(
  ActionTypes.RECEIVE_MESSAGE_FAILURE
);

export const markAsReadAction = createAction(
  ActionTypes.MARK_AS_READ,
  props<{unread: string[]}>()
);

export const markAsReadActionSuccessAction = createAction(
  ActionTypes.MARK_AS_READ_SUCCESS,
  props<{marked: {messageId: string; readAt: string}[]}>()
);
export const markAsReadActionFailureAction = createAction(
  ActionTypes.MARK_AS_READ_FAILURE
);

export const conversationRemovedAction = createAction(
  ActionTypes.CONVERSATION_REMOVED,
  props<{conversationId: string}>()
);
