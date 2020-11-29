import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {IdentityService} from 'src/app/identity/services/identity.service';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {
  signInAction,
  signInSuccessAction,
  signInFailureAction,
} from '../actions/signIn.actions';
import {switchMap, map, catchError, tap} from 'rxjs/operators';
import {of} from 'rxjs';
import {PersistanceService} from '../../../shared/services/persistance.service';

@Injectable()
export class SignInEffect {
  $signIn = createEffect(() =>
    this.actions$.pipe(
      ofType(signInAction),
      switchMap((action) =>
        this.identityService.signIn(action.request).pipe(
          map((r) => signInSuccessAction({currentUser: r})),
          catchError((response) =>
            of(
              signInFailureAction({
                error: {
                  code: response.error.code,
                  message: response.error.message,
                  statusCode: response.status,
                },
              })
            )
          )
        )
      )
    )
  );

  $signInSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(signInSuccessAction),
        tap((x) => {
          this.toastService.showSuccess('You have successfully logged in');
        }),
        tap((x) => {
          this.persistanceService.set('currentUser', x.currentUser);
        })
      ),
    {dispatch: false}
  );

  $signInFailure = createEffect(
    () =>
      this.actions$.pipe(
        ofType(signInFailureAction),
        tap((x) => this.toastService.showError(x.error.message))
      ),
    {dispatch: false}
  );

  constructor(
    private actions$: Actions,
    private identityService: IdentityService,
    private toastService: ToastService,
    private persistanceService: PersistanceService
  ) {}
}
