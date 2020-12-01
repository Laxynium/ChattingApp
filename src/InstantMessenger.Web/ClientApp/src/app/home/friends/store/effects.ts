import {Injectable} from '@angular/core';
import {createEffect, ofType, Actions} from '@ngrx/effects';
import {of} from 'rxjs';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {FriendsService} from 'src/app/home/friends/services/friends.service';
import {
  getPendingInvitationsAction,
  getPendingInvitationsFailureAction,
  getPendingInvitationsSuccessAction,
  sendInvitationAction,
  sendInvitationFailureAction,
  sendInvitationSuccessAction,
} from 'src/app/home/friends/store/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class SendInvitationEffect {
  $sendInvitation = createEffect(() =>
    this.actions$.pipe(
      ofType(sendInvitationAction),
      switchMap((request) => {
        return this.friendsService.sendInvitation(request.nickname).pipe(
          map(() => sendInvitationSuccessAction()),
          catchError((response) =>
            of(
              sendInvitationFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );
  $sendInvitationSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(sendInvitationSuccessAction),
        tap(() => {
          this.toastService.showSuccess('Friendship request has been sent');
        })
      ),
    {dispatch: false}
  );

  $getPendingInvitations = createEffect(() =>
    this.actions$.pipe(
      ofType(getPendingInvitationsAction),
      switchMap(() => {
        return this.friendsService.getAllInvitations().pipe(
          map((invitations) =>
            getPendingInvitationsSuccessAction({invitations: invitations})
          ),
          catchError((response) =>
            of(
              getPendingInvitationsFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private friendsService: FriendsService,
    private toastService: ToastService
  ) {}
}
