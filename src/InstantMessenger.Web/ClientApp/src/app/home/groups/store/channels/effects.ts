import {HttpErrorResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {ChannelsService} from 'src/app/home/groups/services/channels.service';
import {
  getChannelRolePermissionOverridesAction,
  getChannelRolePermissionOverridesFailureAction,
  getChannelRolePermissionOverridesSuccessAction,
  updateChannelRolePermissionOverridesAction,
  updateChannelRolePermissionOverridesFailureAction,
  updateChannelRolePermissionOverridesSuccessAction,
} from 'src/app/home/groups/store/channels/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class ChannelsEffects {
  $updateChannelRolePermissionOverrides = createEffect(() =>
    this.actions$.pipe(
      ofType(updateChannelRolePermissionOverridesAction),
      switchMap((request) => {
        return this.ChannelsService.updateRolePermissionOverrides(request).pipe(
          map((r) =>
            updateChannelRolePermissionOverridesSuccessAction(request)
          ),
          catchError((response: HttpErrorResponse) =>
            of(
              updateChannelRolePermissionOverridesFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );
  $updateChannelRolePermissionSucessOverrides = createEffect(
    () =>
      this.actions$.pipe(
        ofType(updateChannelRolePermissionOverridesSuccessAction),
        tap((r) => {
          this.toasts.showSuccess(
            'Role permissions overrides updated successfully.'
          );
        })
      ),
    {
      dispatch: false,
    }
  );
  $getChannelRolePermissionOverrides = createEffect(() =>
    this.actions$.pipe(
      ofType(getChannelRolePermissionOverridesAction),
      switchMap((request) => {
        return this.ChannelsService.getRolePermissionOverrides({
          groupId: request.groupId,
          channelId: request.channelId,
          roleId: request.roleId,
        }).pipe(
          map((r) =>
            getChannelRolePermissionOverridesSuccessAction({
              groupId: request.groupId,
              roleId: request.roleId,
              channelId: request.channelId,
              overrides: r,
            })
          ),
          catchError((response: HttpErrorResponse) =>
            of(
              getChannelRolePermissionOverridesFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private ChannelsService: ChannelsService,
    private toasts: ToastService
  ) {}
}
