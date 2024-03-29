import {createSelector} from '@ngrx/store';
import {
  Message,
  messagesAdapter,
  MessagesState,
} from 'src/app/home/groups/store/messages/message.reducer';
import {List} from 'immutable';
import {messagesStateSelector} from 'src/app/home/groups/store/selectors';

const {selectAll} = messagesAdapter.getSelectors();

export const messagesSelector = createSelector(
  messagesStateSelector,
  (s: MessagesState): Message[] =>
    List(selectAll(s))
      .sortBy((x) => x.createdAt)
      .map(
        (v) =>
          <Message>{
            messageId: v.messageId,
            content: v.content,
            channelId: v.channelId,
            groupId: v.groupId,
            senderId: v.senderId,
            createdAt: v.createdAt,
            senderAvatar: v.senderAvatar ?? 'assets/profile-placeholder.png',
            senderName: v.senderName,
          }
      )
      .toArray() ?? []
);
