import {createSelector} from '@ngrx/store';
import {MessageDto} from '../types/message';
import {messagesAdapter, MessagesState} from 'src/app/home/groups/store/messages/message.reducer';
import {List} from 'immutable';
import {messagesStateSelector} from 'src/app/home/groups/store/selectors';

const {selectAll} = messagesAdapter.getSelectors();

export const messagesSelector = createSelector(
  messagesStateSelector,
  (s: MessagesState): MessageDto[] =>
    List(selectAll(s))
      .sortBy((x) => x.createdAt)
      .map(
        (v) =>
          <MessageDto>{
            messageId: v.id,
            content: v.content,
            channelId: v.channelId,
            groupId: v.groupId,
            senderId: v.senderId,
            createdAt: v.createdAt.toString(),
            senderAvatar: v.senderAvatar ?? 'assets/profile-placeholder.png',
            senderName: 'test',
          }
      )
      .toArray() ?? []
);
