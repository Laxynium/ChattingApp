import {HttpErrorResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {catchError, map, mergeMap, switchMap, tap} from 'rxjs/operators';
import {RolesService} from 'src/app/home/groups/services/roles.service';
import {
  createRoleAction,
  createRoleFailureAction,
  createRoleSuccessAction,
  getRolePermissionsAction,
  getRolePermissionsFailureAction,
  getRolePermissionsSuccessAction,
  getRolesAction,
  getRolesFailureAction,
  getRolesSuccessAction,
  removeRoleAction,
  removeRoleFailureAction,
  removeRoleSuccessAction,
  updateRolePermissionsAction,
  updateRolePermissionsFailureAction,
  updateRolePermissionsSuccessAction,
} from 'src/app/home/groups/store/roles/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {mapToError} from 'src/app/shared/types/error.response';
import {v4 as guid} from 'uuid';

@Injectable()
export class RolesEffects {
  $getRoles = createEffect(() =>
    this.actions$.pipe(
      ofType(getRolesAction),
      switchMap(({groupId}) => {
        const request = {
          groupId: groupId,
        };
        return this.rolesService.getRoles(request).pipe(
          map((r) => getRolesSuccessAction({roles: r})),
          catchError((response: HttpErrorResponse) =>
            of(
              getRolesFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $getRolePermissions = createEffect(() =>
    this.actions$.pipe(
      ofType(getRolePermissionsAction),
      switchMap((r) => {
        return this.rolesService.getRolePermissions(r).pipe(
          map((r) => getRolePermissionsSuccessAction({roles: r})),
          catchError((response: HttpErrorResponse) =>
            of(
              getRolePermissionsFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $updateRolePermissions = createEffect(() =>
    this.actions$.pipe(
      ofType(updateRolePermissionsAction),
      switchMap((request) => {
        return this.rolesService.updateRolePermissions(request).pipe(
          mergeMap(() => [
            updateRolePermissionsSuccessAction(request),
            getRolePermissionsAction({
              groupId: request.groupId,
              roleId: request.roleId,
            }),
          ]),
          catchError((response: HttpErrorResponse) =>
            of(
              updateRolePermissionsFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $updateRolePermissionsSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(updateRolePermissionsSuccessAction),
        tap(() => {
          this.toasts.showSuccess('Permissions updated successfully');
        })
      ),
    {dispatch: false}
  );

  $createRole = createEffect(() =>
    this.actions$.pipe(
      ofType(createRoleAction),
      switchMap(({groupId, roleName}) => {
        const request = {
          groupId: groupId,
          roleId: guid(),
          name: roleName,
        };
        return this.rolesService.createRole(request).pipe(
          map((r) => createRoleSuccessAction({role: r})),
          catchError((response: HttpErrorResponse) =>
            of(
              createRoleFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $createRoleSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(createRoleSuccessAction),
        tap(({}) => {
          this.toasts.showSuccess(`Role successfully created`);
        })
      ),
    {dispatch: false}
  );

  $removeRole = createEffect(() =>
    this.actions$.pipe(
      ofType(removeRoleAction),
      switchMap(({roleId, groupId}) => {
        const request = {
          groupId: groupId,
          roleId: roleId,
        };
        return this.rolesService.removeRole(request).pipe(
          map(() => removeRoleSuccessAction(request)),
          catchError((response: HttpErrorResponse) =>
            of(
              removeRoleFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $removeRoleSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(removeRoleSuccessAction),
        tap(() => {
          this.toasts.showSuccess(`Role successfully removed`);
        })
      ),
    {dispatch: false}
  );

  constructor(
    private actions$: Actions,
    private rolesService: RolesService,
    private toasts: ToastService
  ) {}
}
