import {Injectable} from '@angular/core';
import {createEffect, ofType, Actions} from '@ngrx/effects';
import {of} from 'rxjs';
import {switchMap, catchError, map} from 'rxjs/operators';
import {InvitationsService} from 'src/app/home/groups/services/invitations.service';
import {} from 'src/app/home/groups/store/groups/actions';
import {
  generateInvitationAction,
  generateInvitationSuccessAction,
  generateInvitationFailureAction,
  revokeInvitationAction,
  revokeInvitationSuccessAction,
  revokeInvitationFailureAction,
  getInvitationsAction,
  getInvitationsSuccessAction,
  getInvitationsFailureAction,
} from 'src/app/home/groups/store/invitations/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {mapToError} from 'src/app/shared/types/error.response';
import {v4 as guid} from 'uuid';

@Injectable()
export class InvitationsEffects {
  $generateInvitation = createEffect(() =>
    this.actions$.pipe(
      ofType(generateInvitationAction),
      switchMap((request) => {
        return this.invitationsService
          .generateInvitation({
            groupId: request.groupId,
            invitationId: guid(),
            expirationTime: request.expirationTime,
            usageCounter: request.usageCounter,
          })
          .pipe(
            map((r) => {
              return generateInvitationSuccessAction({
                groupId: r.groupId,
                invitationId: r.invitationId,
                code: r.code,
              });
            }),
            catchError((response) =>
              of(
                generateInvitationFailureAction(),
                requestFailedAction({error: mapToError(response)})
              )
            )
          );
      })
    )
  );

  $revokeInvitation = createEffect(() =>
    this.actions$.pipe(
      ofType(revokeInvitationAction),
      switchMap((r) => {
        return this.invitationsService.revokeInvitation(r).pipe(
          map(() =>
            revokeInvitationSuccessAction({invitationId: r.invitationId})
          ),
          catchError((response) =>
            of(
              revokeInvitationFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $getInvitations = createEffect(() =>
    this.actions$.pipe(
      ofType(getInvitationsAction),
      switchMap(({groupId}) => {
        return this.invitationsService.getInvitations({groupId}).pipe(
          map((r) => getInvitationsSuccessAction({invitations: r})),
          catchError((response) =>
            of(
              getInvitationsFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );
  constructor(
    private actions$: Actions,
    private invitationsService: InvitationsService
  ) {}
}
