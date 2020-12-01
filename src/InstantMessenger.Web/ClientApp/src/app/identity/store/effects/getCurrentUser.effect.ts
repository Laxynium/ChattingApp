import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {switchMap, catchError, map, tap} from 'rxjs/operators';
import {IdentityService} from 'src/app/identity/services/identity.service';
import {
  getCurrentUser,
  getCurrentUserSuccess,
} from 'src/app/identity/store/actions/getCurrentUser.actions';
import {} from 'src/app/identity/store/actions/signIn.actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class GetProfileEffect {
  $getProfile = createEffect(() =>
    this.actions$.pipe(
      ofType(getCurrentUser),
      switchMap(() =>
        this.identityService.getCurrentUser().pipe(
          map((u) => getCurrentUserSuccess({user: u})),
          catchError((response) =>
            of(requestFailedAction({error: mapToError(response)}))
          )
        )
      )
    )
  );
  constructor(
    private actions$: Actions,
    private identityService: IdentityService
  ) {}
}
