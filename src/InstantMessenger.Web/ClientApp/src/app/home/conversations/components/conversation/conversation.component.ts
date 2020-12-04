import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Params} from '@angular/router';
import {select, Store} from '@ngrx/store';
import {Observable, zip} from 'rxjs';
import {filter, first, map, tap} from 'rxjs/operators';
import {
  changeConversationAction,
  markAsReadAction,
  sendMessageAction,
} from 'src/app/home/conversations/store/actions';
import {
  currentConversationMessagesSelector,
  currentConversationSelector,
  unreadMessagesSelector,
} from 'src/app/home/conversations/store/selectors';
import {
  ConversationInterface,
  ConversationMessageInterface,
} from 'src/app/home/conversations/types/stateTypes/Conversation.interface';
import {currentUserSelector} from 'src/app/identity/store/selectors';
import {ScrollToBottomDirective} from 'src/app/shared/directives/scroll-to-bottom.directive';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';

@Component({
  selector: 'app-conversations',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss'],
})
export class ConversationComponent implements OnInit {
  @ViewChild(ScrollToBottomDirective) scrollMe;

  content: string;
  $messages: Observable<ConversationMessageInterface[]>;
  $conversation: Observable<ConversationInterface>;
  $me: Observable<CurrentUserInterface>;
  constructor(private store: Store, activatedRoute: ActivatedRoute) {
    activatedRoute.params.subscribe((x: Params) => {
      const conversationId = x.id;
      if (conversationId) {
        this.store.dispatch(
          changeConversationAction({conversationId: conversationId})
        );
      }
    });
  }

  ngOnInit(): void {
    this.$conversation = this.store.pipe(
      select(currentConversationSelector),
      filter((x) => x != null)
    );
    this.$messages = this.store.pipe(
      select(currentConversationMessagesSelector),
      tap((_) => {
        if (this.scrollMe) {
          this.scrollMe.scrollToBottom();
        }
        this.markAsRead();
      })
    );
    this.$me = this.store.pipe(select(currentUserSelector));
    this.markAsRead();
  }

  send() {
    this.$conversation.pipe(first()).subscribe((x) => {
      this.store.dispatch(
        sendMessageAction({conversationId: x.id, content: this.content})
      );
      this.content = '';
    });
  }

  private markAsRead() {
    zip(
      this.$conversation,
      this.$me,
      this.store.pipe(select(unreadMessagesSelector))
    )
      .pipe(
        map(([c, me, unread]) => {
          return {unread: unread(c.id, me.id)};
        }),
        first()
      )
      .subscribe((r) => {
        this.store.dispatch(
          markAsReadAction({unread: r.unread.map((x) => x.id)})
        );
      });
  }
}
