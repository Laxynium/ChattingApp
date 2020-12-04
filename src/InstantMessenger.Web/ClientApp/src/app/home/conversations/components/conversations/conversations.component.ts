import {Component, OnInit} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {Observable, pipe} from 'rxjs';
import {map} from 'rxjs/operators';
import {
  changeConversationAction,
  getLatestConversationsAction,
} from 'src/app/home/conversations/store/actions';
import {latestConversationsSelector} from 'src/app/home/conversations/store/selectors';
import {ConversationInterface} from 'src/app/home/conversations/types/stateTypes/Conversation.interface';
import {currentUserSelector} from 'src/app/identity/store/selectors';

@Component({
  selector: 'app-conversations',
  templateUrl: './conversations.component.html',
  styleUrls: ['./conversations.component.scss'],
})
export class ConversationsComponent implements OnInit {
  $latestConversations: Observable<ConversationInterface[]>;
  $meId: Observable<string>;
  constructor(private store: Store) {
    this.$latestConversations = this.store.pipe(
      select(latestConversationsSelector)
    );
    this.$meId = this.store.pipe(
      select(currentUserSelector),
      map((u) => u.id)
    );
  }

  ngOnInit(): void {
    this.store.dispatch(getLatestConversationsAction({count: 10}));
  }

  changeConversation(conversationId) {
    this.store.dispatch(changeConversationAction({conversationId}));
  }
}
