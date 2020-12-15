import {HttpErrorResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {ChannelsService} from 'src/app/home/groups/services/channels.service';
import {
  getChannelMemberPermissionOverridesAction,
  getChannelMemberPermissionOverridesFailureAction,
  getChannelMemberPermissionOverridesSuccessAction,
  getChannelRolePermissionOverridesAction,
  getChannelRolePermissionOverridesFailureAction,
  getChannelRolePermissionOverridesSuccessAction,
  renameChannelAction,
  renameChannelFailureAction,
  renameChannelSuccessAction,
  updateChannelMemberPermissionOverridesAction,
  updateChannelMemberPermissionOverridesFailureAction,
  updateChannelMemberPermissionOverridesSuccessAction,
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
        return this.channelsService.updateRolePermissionOverrides(request).pipe(
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
        return this.channelsService
          .getRolePermissionOverrides({
            groupId: request.groupId,
            channelId: request.channelId,
            roleId: request.roleId,
          })
          .pipe(
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

  $updateChannelMemberPermissionOverrides = createEffect(() =>
    this.actions$.pipe(
      ofType(updateChannelMemberPermissionOverridesAction),
      switchMap((request) => {
        return this.channelsService
          .updateMemberPermissionOverrides(request)
          .pipe(
            map((r) =>
              updateChannelMemberPermissionOverridesSuccessAction(request)
            ),
            catchError((response: HttpErrorResponse) =>
              of(
                updateChannelMemberPermissionOverridesFailureAction(),
                requestFailedAction({error: mapToError(response)})
              )
            )
          );
      })
    )
  );
  $updateChannelMemberPermissionSucessOverrides = createEffect(
    () =>
      this.actions$.pipe(
        ofType(updateChannelMemberPermissionOverridesSuccessAction),
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
  $getChannelMemberPermissionOverrides = createEffect(() =>
    this.actions$.pipe(
      ofType(getChannelMemberPermissionOverridesAction),
      switchMap((request) => {
        return this.channelsService.getMemberPermissionOverrides(request).pipe(
          map((r) =>
            getChannelMemberPermissionOverridesSuccessAction({
              groupId: request.groupId,
              memberUserId: request.memberUserId,
              channelId: request.channelId,
              overrides: r,
            })
          ),
          catchError((response: HttpErrorResponse) =>
            of(
              getChannelMemberPermissionOverridesFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $renameChannel = createEffect(() =>
    this.actions$.pipe(
      ofType(renameChannelAction),
      switchMap((request) => {
        return this.channelsService.renameChannel(request.channel).pipe(
          map(() => renameChannelSuccessAction({channel: request.channel})),
          catchError((response: HttpErrorResponse) =>
            of(
              renameChannelFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $renameChannelSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(renameChannelSuccessAction),
        tap(() => {
          this.toasts.showSuccess(`Channel successfully renamed`);
        })
      ),
    {dispatch: false}
  );

  constructor(
    private actions$: Actions,
    private channelsService: ChannelsService,
    private toasts: ToastService
  ) {}
}
