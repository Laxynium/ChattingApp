import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {Observable, zip} from 'rxjs';
import {Store, select} from '@ngrx/store';
import {messagesSelector} from 'src/app/home/groups/store/messages/selectors';
import {MessageDto} from 'src/app/home/groups/store/types/message';
import {
  getMessagesAction,
  sendMessageAction,
} from '../../store/messages/actions';
import {currentUserSelector} from '../../../../identity/store/selectors';
import {first, map, mergeMap, tap} from 'rxjs/operators';
import {ChannelDto} from '../../services/responses/group.dto';
import {v4 as guid} from 'uuid';
import {ScrollToBottomDirective} from 'src/app/shared/directives/scroll-to-bottom.directive';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss'],
})
export class MessagesComponent implements OnInit {
  @ViewChild(ScrollToBottomDirective) scrollMe;

  messageContent: string = '';
  @Input() channel: ChannelDto;
  $messages: Observable<(MessageDto & {isMine: boolean})[]>;

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
    this.store.dispatch(
      getMessagesAction({
        groupId: this.channel.groupId,
        channelId: this.channel.channelId,
      })
    );
  }

  sendMessage() {
    this.store.dispatch(
      sendMessageAction({
        message: {
          groupId: this.channel.groupId,
          channelId: this.channel.channelId,
          messageId: guid(),
          content: this.messageContent,
        },
      })
    );
    this.messageContent = '';
  }
}
