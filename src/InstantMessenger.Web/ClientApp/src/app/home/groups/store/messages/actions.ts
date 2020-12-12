import {createAction, props} from '@ngrx/store';
import {MessageDto} from '../types/message';
export enum ActionTypes {
  GET_MESSAGES = '[Groups] Get messages',
  GET_MESSAGES_SUCCESS = '[Groups] Get messages success',
  GET_MESSAGES_FAILURE = '[Groups] Get messages failure',

  SEND_MESSAGE = '[Groups] Send messages',
  SEND_MESSAGE_SUCCESS = '[Groups] Send messages success',
  SEND_MESSAGE_FAILURE = '[Groups] Send messages failure',
}

export const getMessagesAction = createAction(
  ActionTypes.GET_MESSAGES,
  props<{groupId: string; channelId: string}>()
);
export const getMessagesSuccessAction = createAction(
  ActionTypes.GET_MESSAGES_SUCCESS,
  props<{messages: MessageDto[]}>()
);
export const getMessagesFailureAction = createAction(
  ActionTypes.GET_MESSAGES_FAILURE
);

export const sendMessageAction = createAction(
  ActionTypes.SEND_MESSAGE,
  props<{
    message: {
      groupId: string;
      channelId: string;
      messageId: string;
      content: string;
    };
  }>()
);
export const sendMessageSuccessAction = createAction(
  ActionTypes.SEND_MESSAGE_SUCCESS,
  props<{
    message: MessageDto;
  }>()
);
export const sendMessageFailureAction = createAction(
  ActionTypes.SEND_MESSAGE_FAILURE
);
