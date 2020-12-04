import {createAction, props} from '@ngrx/store';
import {ConversationsStateInterface} from 'src/app/home/conversations/store/reducers';
import {ConversationResponseInterface} from 'src/app/home/conversations/types/responseTypes/conversation.response';
import {
  ConversationInterface,
  ConversationMessageInterface,
} from 'src/app/home/conversations/types/stateTypes/Conversation.interface';

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
}

export const getLatestConversationsAction = createAction(
  ActionTypes.GET_LATEST_CONVERSATIONS,
  props<{count: number}>()
);

export const getLatestConversationsSuccessAction = createAction(
  ActionTypes.GET_LATEST_CONVERSATIONS_SUCCESS,
  props<{conversations: ConversationInterface[]}>()
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
