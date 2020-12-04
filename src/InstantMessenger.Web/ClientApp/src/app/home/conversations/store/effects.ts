import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {ConversationsService} from 'src/app/home/conversations/services/conversations.service';
import {
  changeConversationAction,
  changeConversationSuccessAction,
  getLatestConversationsAction,
  getLatestConversationsSuccessAction,
  markAsReadAction,
  receiveMessageSuccessAction,
  sendMessageAction,
  sendMessageSuccessAction,
} from 'src/app/home/conversations/store/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class ConversationsEffects {
  $getLatestConversations = createEffect(() =>
    this.actions$.pipe(
      ofType(getLatestConversationsAction),
      switchMap((params) =>
        this.conversationsService.getLatestConversations(params.count).pipe(
          map((r) =>
            getLatestConversationsSuccessAction({
              conversations: r.map((x) => ({
                id: x.conversationId,
                firstParticipant: x.firstParticipant,
                secondParticipant: x.secondParticipant,
                messages: [],
                unreadCount: x.unreadMessagesCount,
              })),
            })
          ),
          catchError((response) =>
            of(requestFailedAction({error: mapToError(response)}))
          )
        )
      )
    )
  );

  $changeConversation = createEffect(() =>
    this.actions$.pipe(
      ofType(changeConversationAction),
      switchMap(({conversationId}) =>
        this.conversationsService.getConversation(conversationId).pipe(
          map((r) => changeConversationSuccessAction({conversation: r})),
          catchError((response) =>
            of(requestFailedAction({error: mapToError(response)}))
          )
        )
      )
    )
  );
  $changeConversationSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(changeConversationSuccessAction),
        tap(({conversation: c}) => {
          this.router.navigateByUrl(`/conversations/${c.conversationId}`, {
            state: {
              id: c.conversationId,
            },
          });
        })
      ),
    {dispatch: false}
  );
  $sentMessage = createEffect(() =>
    this.actions$.pipe(
      ofType(sendMessageAction),
      switchMap(({conversationId, content}) =>
        this.conversationsService.sendMessage(conversationId, content).pipe(
          map((r) =>
            sendMessageSuccessAction({
              conversationId: conversationId,
              message: {
                id: r.messageId,
                conversationId: conversationId,
                body: r.content,
                createdAt: r.createdAt,
                fromUserId: r.from,
                toUserId: r.to,
                readAt: r.readAt,
              },
            })
          ),
          catchError((r) => of(requestFailedAction({error: mapToError(r)})))
        )
      )
    )
  );

  $markAsRead = createEffect(() =>
    this.actions$.pipe(
      ofType(markAsReadAction),
      switchMap((x) =>
        this.conversationsService.markAsRead(x.unread).pipe(
          map((_) => getLatestConversationsAction({count: 10})),
          catchError((r) => of(requestFailedAction({error: mapToError(r)})))
        )
      )
    )
  );

  $receiveMessage = createEffect(() =>
    this.actions$.pipe(
      ofType(receiveMessageSuccessAction),
      switchMap(() => of(getLatestConversationsAction({count: 10})))
    )
  );

  constructor(
    private actions$: Actions,
    private conversationsService: ConversationsService,
    private router: Router
  ) {}
}
