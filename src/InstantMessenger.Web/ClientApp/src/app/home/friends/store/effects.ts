import {Injectable} from '@angular/core';
import {createEffect, ofType, Actions} from '@ngrx/effects';
import {of} from 'rxjs';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {FriendsService} from 'src/app/home/friends/services/friends.service';
import {
  acceptInvitationAction,
  acceptInvitationFailureAction,
  acceptInvitationSuccessAction,
  cancelInvitationAction,
  cancelInvitationFailureAction,
  cancelInvitationSuccessAction,
  getFriendsAction,
  getFriendsFailureAction,
  getFriendsSuccessAction,
  getPendingInvitationsAction,
  getPendingInvitationsFailureAction,
  getPendingInvitationsSuccessAction,
  rejectInvitationAction,
  rejectInvitationFailureAction,
  rejectInvitationSuccessAction,
  sendInvitationAction,
  sendInvitationFailureAction,
  sendInvitationSuccessAction,
} from 'src/app/home/friends/store/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class GetFriendsData {
  $getPendingInvitations = createEffect(() =>
    this.actions$.pipe(
      ofType(getPendingInvitationsAction),
      switchMap(() =>
        this.friendsService.getAllInvitations().pipe(
          map((invitations) =>
            getPendingInvitationsSuccessAction({invitations: invitations})
          ),
          catchError((response) =>
            of(
              getPendingInvitationsFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        )
      )
    )
  );

  $getFriendships = createEffect(() =>
    this.actions$.pipe(
      ofType(getFriendsAction),
      switchMap(() => {
        return this.friendsService.getFriends().pipe(
          map((friends) => getFriendsSuccessAction({friends: friends})),
          catchError((response) =>
            of(
              getFriendsFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private friendsService: FriendsService
  ) {}
}

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

  constructor(
    private actions$: Actions,
    private friendsService: FriendsService,
    private toastService: ToastService
  ) {}
}

@Injectable()
export class AcceptInvitationEffect {
  $acceptInvitation = createEffect(() =>
    this.actions$.pipe(
      ofType(acceptInvitationAction),
      switchMap((request) => {
        return this.friendsService.acceptInvitation(request.id).pipe(
          map(() => acceptInvitationSuccessAction({id: request.id})),
          catchError((response) =>
            of(
              acceptInvitationFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );
  $acceptInvitationSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(acceptInvitationSuccessAction),
        tap(() => {
          this.toastService.showSuccess('Friendship request has been accepted');
        })
      ),
    {dispatch: false}
  );

  constructor(
    private actions$: Actions,
    private friendsService: FriendsService,
    private toastService: ToastService
  ) {}
}

@Injectable()
export class RejectInvitationEffect {
  $rejectInvitation = createEffect(() =>
    this.actions$.pipe(
      ofType(rejectInvitationAction),
      switchMap((request) => {
        return this.friendsService.rejectInvitation(request.id).pipe(
          map(() => rejectInvitationSuccessAction({id: request.id})),
          catchError((response) =>
            of(
              rejectInvitationFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );
  $rejectInvitationSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(rejectInvitationSuccessAction),
        tap(() => {
          this.toastService.showSuccess('Friendship request has been rejected');
        })
      ),
    {dispatch: false}
  );

  constructor(
    private actions$: Actions,
    private friendsService: FriendsService,
    private toastService: ToastService
  ) {}
}

@Injectable()
export class CancelInvitationEffect {
  $cancelInvitation = createEffect(() =>
    this.actions$.pipe(
      ofType(cancelInvitationAction),
      switchMap((request) => {
        return this.friendsService.cancelInvitation(request.id).pipe(
          map(() => cancelInvitationSuccessAction({id: request.id})),
          catchError((response) =>
            of(
              cancelInvitationFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );
  $cancelInvitationSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(cancelInvitationSuccessAction),
        tap(() => {
          this.toastService.showSuccess('Friendship request has been rejected');
        })
      ),
    {dispatch: false}
  );

  constructor(
    private actions$: Actions,
    private friendsService: FriendsService,
    private toastService: ToastService
  ) {}
}
