import {Action, createReducer, on} from '@ngrx/store';
import {
  changeConversationAction,
  changeConversationSuccessAction,
  conversationRemovedAction,
  getLatestConversationsSuccessAction,
  markAsReadActionSuccessAction,
  receiveMessageSuccessAction,
  sendMessageSuccessAction,
} from 'src/app/home/conversations/store/actions';
import {
  ConversationInterface,
  LatestConversationInterface,
} from 'src/app/home/conversations/types/stateTypes/Conversation.interface';

export interface ConversationsStateInterface {
  latestConversations: LatestConversationInterface[];
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
        messages: [...(s.currentConversation?.messages ?? []), a.message],
      },
    })
  ),
  on(
    markAsReadActionSuccessAction,
    (s, a): ConversationsStateInterface => ({
      ...s,
      currentConversation: {
        ...s.currentConversation,
        messages: replace(
          s.currentConversation.messages,
          s.currentConversation.messages
            .filter((m) => a.marked.map((x) => x.messageId).includes(m.id))
            .map((m) => ({
              ...m,
              readAt: a.marked.find((x) => x.messageId == m.id).readAt,
            })),
          (a, b) => a.id == b.id
        ),
      },
    })
  ),
  on(
    conversationRemovedAction,
    (s, a): ConversationsStateInterface => ({
      ...s,
      latestConversations: s.latestConversations.filter(
        (x) => x.id != a.conversationId
      ),
      currentConversation:
        s.currentConversation.id == a.conversationId
          ? null
          : s.currentConversation,
    })
  )
);

function replace<AType>(
  as: AType[],
  bs: AType[],
  when: (a: AType, b: AType) => boolean
): AType[] {
  return as.reduce((agg, a) => {
    const el = bs.find((x) => when(a, x)) ?? a;
    return [...agg, el];
  }, []);
}

export function reducers(state: ConversationsStateInterface, action: Action) {
  return conversationsReducer(state, action);
}
