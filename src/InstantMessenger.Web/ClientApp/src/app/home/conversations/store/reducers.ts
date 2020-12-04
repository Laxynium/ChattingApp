import {Action, createReducer, on} from '@ngrx/store';
import {
  changeConversationAction,
  changeConversationSuccessAction,
  getLatestConversationsSuccessAction,
  receiveMessageSuccessAction,
  sendMessageSuccessAction,
} from 'src/app/home/conversations/store/actions';
import {ConversationInterface} from 'src/app/home/conversations/types/stateTypes/Conversation.interface';

export interface ConversationsStateInterface {
  latestConversations: ConversationInterface[];
  currentConversation: ConversationInterface | null;
}

const initialState: ConversationsStateInterface = {
  latestConversations: [],
  currentConversation: null,
};

const conversationsReducer = createReducer(
  initialState,
  on(getLatestConversationsSuccessAction, (s, a) => ({
    ...s,
    latestConversations: [...a.conversations],
  })),
  on(changeConversationAction, (s, a) => ({
    ...s,
  })),
  on(
    changeConversationSuccessAction,
    (s, a): ConversationsStateInterface => ({
      ...s,
      currentConversation: {
        id: a.conversation.conversationId,
        firstParticipant: a.conversation.firstParticipant,
        secondParticipant: a.conversation.secondParticipant,
        messages: a.conversation.messages.map((m) => {
          return {
            id: m.messageId,
            conversationId: a.conversation.conversationId,
            body: m.content,
            createdAt: m.createdAt,
            fromUserId: m.from,
            toUserId: m.to,
            readAt: m.readAt,
          };
        }),
      },
    })
  ),
  on(
    sendMessageSuccessAction,
    (s, a): ConversationsStateInterface => {
      return {
        ...s,
        currentConversation: {
          ...s.currentConversation,
          messages: [...s.currentConversation.messages, a.message],
        },
      };
    }
  ),
  on(
    receiveMessageSuccessAction,
    (s, a): ConversationsStateInterface => ({
      ...s,
      currentConversation: {
        ...s.currentConversation,
        messages: [...s.currentConversation.messages, a.message],
      },
    })
  )
);

export function reducers(state: ConversationsStateInterface, action: Action) {
  return conversationsReducer(state, action);
}
