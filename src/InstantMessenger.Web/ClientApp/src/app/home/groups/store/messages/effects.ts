import {HttpErrorResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {
  getMessagesAction,
  getMessagesFailureAction,
  getMessagesSuccessAction,
} from 'src/app/home/groups/store/messages/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {mapToError} from 'src/app/shared/types/error.response';
import {MesssagesService} from '../../services/messages.service';
import {
  sendMessageAction,
  sendMessageSuccessAction,
  sendMessageFailureAction,
} from './actions';

@Injectable()
export class MessagesEffects {
  $getMessages = createEffect(() =>
    this.actions$.pipe(
      ofType(getMessagesAction),
      switchMap((request) => {
        return this.membersService.getMessages(request).pipe(
          map((r) => getMessagesSuccessAction({messages: r})),
          catchError((response: HttpErrorResponse) =>
            of(
              getMessagesFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );
  $sendMessage = createEffect(() =>
    this.actions$.pipe(
      ofType(sendMessageAction),
      switchMap((request) => {
        return this.membersService.sendMessage(request.message).pipe(
          map((message) => sendMessageSuccessAction({message: message})),
          catchError((response: HttpErrorResponse) =>
            of(
              sendMessageFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private membersService: MesssagesService,
    private toasts: ToastService
  ) {}
}
