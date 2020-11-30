import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
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
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {HttpErrorResponse} from '@angular/common/http';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class ActivateEffect {
  activate$ = createEffect(() =>
    this.actions$.pipe(
      ofType(activateAction),
      switchMap(({request}) =>
        this.identityService.activate(request).pipe(
          map(() => activateSuccessAction()),
          catchError((r: HttpErrorResponse) =>
            of(
              activateFailureAction(),
              requestFailedAction({error: mapToError(r)})
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

  constructor(
    private actions$: Actions,
    private identityService: IdentityService,
    private toastService: ToastService,
    private router: Router
  ) {}
}
