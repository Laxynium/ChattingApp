import {createFeatureSelector, createSelector} from '@ngrx/store';
import {ConversationsStateInterface} from 'src/app/home/conversations/store/reducers';
import {AppStateInterface} from 'src/app/shared/types/appState.interface';
import {withDefaultAvatar} from 'src/app/shared/types/user.interface';

export const conversationsFeatureSelector = createFeatureSelector<
  AppStateInterface,
  ConversationsStateInterface
>('conversations');

export const currentConversationSelector = createSelector(
  conversationsFeatureSelector,
  (state: ConversationsStateInterface) => ({
    ...state.currentConversation,
    firstParticipant: withDefaultAvatar(
      state.currentConversation?.firstParticipant
    ),
    secondParticipant: withDefaultAvatar(
      state.currentConversation?.secondParticipant
    ),
  })
);

export const currentConversationMessagesSelector = createSelector(
  conversationsFeatureSelector,
  (state: ConversationsStateInterface) =>
    state?.currentConversation?.messages ?? []
);

export const latestConversationsSelector = createSelector(
  conversationsFeatureSelector,
  (state: ConversationsStateInterface) =>
    state.latestConversations?.map((c) => ({
      ...c,
      firstParticipant: withDefaultAvatar(c.firstParticipant),
      secondParticipant: withDefaultAvatar(c.secondParticipant),
      unreadCount: c.unreadCount,
    })) ?? []
);

export const unreadMessagesSelector = createSelector(
  conversationsFeatureSelector,
  (state: ConversationsStateInterface) => (
    conversationId: string,
    meId: string
  ) =>
    state.currentConversation?.messages
      .filter((x) => x.conversationId == conversationId)
      .filter((x) => x.toUserId == meId)
      .filter((x) => isNullOrWhitespace(x.readAt)) ?? []
);

function isNullOrWhitespace(input: string) {
  return !input || !input.trim();
}
