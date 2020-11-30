import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {switchMap, tap} from 'rxjs/operators';
import {
  logoutActiion,
  logoutSuccessAction,
} from 'src/app/identity/store/actions/logout.actions';
import {PersistanceService} from 'src/app/shared/services/persistance.service';
import {ToastService} from 'src/app/shared/toasts/toast.service';

@Injectable()
export class LogoutEffect {
  $logout = createEffect(() =>
    this.actions$.pipe(
      ofType(logoutActiion),
      switchMap(() => of(logoutSuccessAction()))
    )
  );
  $logoutSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(logoutSuccessAction),
        tap(() => {
          this.persistanceService.unset('currentUser');
          this.toastService.showSuccess('You have successfully logged out');
          this.router.navigateByUrl('/sign-in');
        })
      ),
    {dispatch: false}
  );
  constructor(
    private actions$: Actions,
    private toastService: ToastService,
    private persistanceService: PersistanceService,
    private router: Router
  ) {}
}
