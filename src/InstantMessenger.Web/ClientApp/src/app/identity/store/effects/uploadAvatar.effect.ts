import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {switchMap, catchError, map, tap} from 'rxjs/operators';
import {IdentityService} from 'src/app/identity/services/identity.service';
import {} from 'src/app/identity/store/actions/signIn.actions';
import {
  uploadAvatar,
  uploadAvatarSuccess,
} from 'src/app/identity/store/actions/uploadAvatar.actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {ToastService} from 'src/app/shared/toasts/toast.service';

@Injectable()
export class UploadAvatarEffect {
  $uploadAvatar = createEffect(() =>
    this.actions$.pipe(
      ofType(uploadAvatar),
      switchMap((action) =>
        this.identityService.uploadAvatar(action.request).pipe(
          map((r) => uploadAvatarSuccess({user: r})),
          catchError((response) =>
            of(
              requestFailedAction({
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

  $uploadAvatarSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(uploadAvatarSuccess),
        tap((x) => {
          this.toastService.showSuccess(
            'Avatar has been successfully uploaded.'
          );
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
