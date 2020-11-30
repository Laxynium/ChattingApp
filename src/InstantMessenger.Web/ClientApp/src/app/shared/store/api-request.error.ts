import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {createAction, props} from '@ngrx/store';
import {tap} from 'rxjs/operators';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {ErrorResponseInterface} from 'src/app/shared/types/error.response';

export enum RequestFailedActionNames {
  REQUEST_FAILED = '[Error Handling] Request failed',
}
export const requestFailedAction = createAction(
  RequestFailedActionNames.REQUEST_FAILED,
  props<{error: ErrorResponseInterface}>()
);

@Injectable()
export class RequestFailedEffect {
  $requestFailed = createEffect(
    () =>
      this.actions$.pipe(
        ofType(requestFailedAction),
        tap((response) => {
          this.toastService.showError(response.error.message); //TODO map codes to proper messages
        })
      ),
    {dispatch: false}
  );

  constructor(private actions$: Actions, private toastService: ToastService) {}
}
