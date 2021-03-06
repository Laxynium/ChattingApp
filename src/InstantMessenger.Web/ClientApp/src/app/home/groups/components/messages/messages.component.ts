import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {Observable} from 'rxjs';
import {Store, select} from '@ngrx/store';
import {messagesSelector} from 'src/app/home/groups/store/messages/selectors';
import {
  getMessagesAction,
  sendMessageAction,
} from 'src/app/home/groups/store/messages/actions';
import {currentUserSelector} from 'src/app/identity/store/selectors';
import {first, map, mergeMap, tap, withLatestFrom} from 'rxjs/operators';
import {v4 as guid} from 'uuid';
import {ScrollToBottomDirective} from 'src/app/shared/directives/scroll-to-bottom.directive';
import {Message} from "src/app/home/groups/store/messages/message.reducer";

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss'],
})
export class MessagesComponent implements OnInit {
  @ViewChild(ScrollToBottomDirective) scrollMe;

  @Input() $groupId: Observable<string>;
  @Input() $channelId: Observable<string>;

  messageContent: string = '';
  $messages: Observable<(Message & {isMine: boolean})[]>;

  constructor(private store: Store) {
    this.$messages = this.store.pipe(
      select(messagesSelector),
      mergeMap((ms) =>
        this.store.pipe(
          select(currentUserSelector),
          map((u) => ms.map((m) => ({...m, isMine: m.senderId == u.id})))
        )
      ),
      tap((_) => {
        if (this.scrollMe) {
          this.scrollMe.scrollToBottom();
        }
      })
    );
  }

  ngOnInit(): void {
    this.$channelId
      .pipe(
        withLatestFrom(this.$groupId),
        tap(([c, g]) => {
          this.store.dispatch(
            getMessagesAction({
              channelId: c,
              groupId: g,
            })
          );
        })
      )
      .subscribe();
  }

  sendMessage() {
    this.$channelId
      .pipe(
        withLatestFrom(this.$groupId),
        tap(([c, g]) => {
          this.store.dispatch(
            sendMessageAction({
              message: {
                groupId: g,
                channelId: c,
                messageId: guid(),
                content: this.messageContent,
              },
            })
          );
        }),
        first()
      )
      .subscribe();

    this.messageContent = '';
  }
}
