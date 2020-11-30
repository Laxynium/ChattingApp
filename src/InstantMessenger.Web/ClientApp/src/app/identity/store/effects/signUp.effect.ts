import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {
  signUpAction,
  signUpFailureAction,
  signUpSuccessAction,
} from 'src/app/identity/store/actions/signUp.actions';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {IdentityService} from 'src/app/identity/services/identity.service';
import {of} from 'rxjs';
import {ToastService} from '../../../shared/toasts/toast.service';
import {Router} from '@angular/router';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {HttpErrorResponse} from '@angular/common/http';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class SignUpEffect {
  signUp$ = createEffect(() =>
    this.actions$.pipe(
      ofType(signUpAction),
      switchMap(({request}) =>
        this.identityService.signUp(request).pipe(
          map(() => signUpSuccessAction()),
          catchError((response: HttpErrorResponse) =>
            of(
              signUpFailureAction(),
              requestFailedAction({error: mapToError(response)})
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
          this.route.navigateByUrl('/sign-in');
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
