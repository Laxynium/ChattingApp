import {HttpErrorResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {IdentityService} from 'src/app/identity/services/identity.service';
import {
  changeNicknameAction,
  changeNicknameSuccessAction,
} from 'src/app/identity/store/actions/changeNickname.actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class ChangeNicknameEffect {
  $changeNickname = createEffect(() =>
    this.actions$.pipe(
      ofType(changeNicknameAction),
      switchMap((action) =>
        this.identityService.changeNickname(action.request.nickname).pipe(
          map((r) => changeNicknameSuccessAction({response: {nickname: r}})),
          catchError((r: HttpErrorResponse) =>
            of(requestFailedAction({error: mapToError(r)}))
          )
        )
      )
    )
  );

  $changeNicknameSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(changeNicknameSuccessAction),
        tap(() => {
          this.toastService.showSuccess('Nickname changed successfully');
        })
      ),
    {dispatch: false}
  );
  constructor(
    private actions$: Actions,
    private toastService: ToastService,
    private identityService: IdentityService
  ) {}
}
