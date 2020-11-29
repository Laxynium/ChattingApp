import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {
  signUpAction,
  signUpFailureAction,
  signUpSuccessAction,
} from 'src/app/identity/store/actions/signUp.actions';
import {catchError, map, mergeMap, switchMap, tap} from 'rxjs/operators';
import {IdentityService} from 'src/app/identity/services/identity.service';
import {of} from 'rxjs';
import {ToastService} from '../../../shared/toasts/toast.service';
import {Router} from '@angular/router';

@Injectable()
export class SignUpEffect {
  signUp$ = createEffect(() =>
    this.actions$.pipe(
      ofType(signUpAction),
      switchMap(({request}) =>
        this.identityService.signUp(request).pipe(
          map(() => signUpSuccessAction()),
          catchError((response) =>
            of(
              signUpFailureAction({
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

  signUpSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(signUpSuccessAction),
        tap(() => {
          this.toastService.showSuccess(
            `Account registered successfully. 
            You will receive activation email soon.`
          );
          this.route.navigateByUrl('/identity/sign-in');
        })
      ),
    {dispatch: false}
  );

  signUpFailure$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(signUpFailureAction),
        tap((action) => {
          this.toastService.showError(action.error.message);
        })
      ),
    {dispatch: false}
  );

  constructor(
    private actions$: Actions,
    private identityService: IdentityService,
    private route: Router,
    private toastService: ToastService
  ) {}
}
