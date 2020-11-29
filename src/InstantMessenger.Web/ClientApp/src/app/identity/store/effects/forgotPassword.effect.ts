import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {IdentityService} from 'src/app/identity/services/identity.service';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {switchMap, map, catchError, tap} from 'rxjs/operators';
import {of} from 'rxjs';
import {Router} from '@angular/router';
import {resetPasswordFailureAction} from '../actions/forgotPassword.actions';
import {
  resetPasswordAction,
  resetPasswordSuccessAction,
} from '../actions/forgotPassword.actions';
import {
  forgotPasswordAction,
  forgotPasswordSuccessAction,
  forgotPasswordFailureAction,
} from '../actions/forgotPassword.actions';

@Injectable()
export class ForgotPasswordEffect {
  $forgotPassword = createEffect(() =>
    this.actions$.pipe(
      ofType(forgotPasswordAction),
      switchMap((action) =>
        this.identityService.forgotPassword(action.request).pipe(
          map((r) => forgotPasswordSuccessAction()),
          catchError(() => of(forgotPasswordFailureAction()))
        )
      )
    )
  );

  $forgotPasswordSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(forgotPasswordSuccessAction, forgotPasswordFailureAction), //we don't show any errors due to security reasons
        tap((x) => {
          this.toastService.showSuccess(
            'We have sent you email to reset password'
          );
        })
      ),
    {dispatch: false}
  );

  $resetPassword = createEffect(() =>
    this.actions$.pipe(
      ofType(resetPasswordAction),
      switchMap((action) =>
        this.identityService.resetPassword(action.request).pipe(
          map(() => resetPasswordSuccessAction()),
          catchError((response) =>
            of(
              resetPasswordFailureAction({
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

  $resetPasswordSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(resetPasswordSuccessAction),
        tap((x) => {
          this.toastService.showSuccess('Your password has been changed.');
        }),
        tap((_) => {
          this.router.navigateByUrl('/sign-in');
        })
      ),
    {dispatch: false}
  );

  $resetPasswordFailure = createEffect(
    () =>
      this.actions$.pipe(
        ofType(resetPasswordFailureAction),
        tap((action) => {
          this.toastService.showError(action.error.message);
        })
      ),
    {dispatch: false}
  );

  constructor(
    private actions$: Actions,
    private identityService: IdentityService,
    private toastService: ToastService,
    private router: Router
  ) {}
}
