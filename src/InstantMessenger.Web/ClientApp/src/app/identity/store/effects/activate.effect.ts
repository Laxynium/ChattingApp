import {Injectable} from '@angular/core';
import {Actions, createEffect, Effect, ofType} from '@ngrx/effects';
import {map, switchMap, catchError, tap} from 'rxjs/operators';
import {
  activateAction,
  activateSuccessAction,
  activateFailureAction,
} from '../actions/activate.actions';
import {IdentityService} from '../../services/identity.service';
import {of} from 'rxjs';
import {ToastService} from '../../../shared/toasts/toast.service';
import {Router} from '@angular/router';

@Injectable()
export class ActivateEffect {
  activate$ = createEffect(() =>
    this.actions$.pipe(
      ofType(activateAction),
      switchMap(({request}) =>
        this.identityService.activate(request).pipe(
          map(() => activateSuccessAction()),
          catchError((response) =>
            of(
              activateFailureAction({
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

  activateSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(activateSuccessAction),
        tap(() => {
          this.toastService.showSuccess('Account activated successfully');
          this.router.navigateByUrl('/identity/sign-in');
        })
      ),
    {dispatch: false}
  );

  activateFailure$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(activateFailureAction),
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
