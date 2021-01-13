import {createSelector} from '@ngrx/store';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {groupsFeatureSelector} from 'src/app/home/groups/store/groups/selectors';
import {MessageDto} from '../types/message';

export const messagesSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): MessageDto[] =>
    s.currentChannelMessages
      .toList()
      .sortBy((x) => x.createdAt)
      .map((v) => ({
        ...v,
        senderAvatar: v.senderAvatar ?? 'assets/profile-placeholder.png',
      }))
      .toArray() ?? []
);
