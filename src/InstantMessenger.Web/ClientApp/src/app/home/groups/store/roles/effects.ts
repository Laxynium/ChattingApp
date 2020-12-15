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
  moveDownRoleAction,
  moveDownRoleFailureAction,
  moveDownRoleSuccessAction,
  moveUpRoleAction,
  moveUpRoleFailureAction,
  moveUpRoleSuccessAction,
  removeRoleAction,
  removeRoleFailureAction,
  removeRoleSuccessAction,
  renameRoleAction,
  renameRoleFailureAction,
  renameRoleSuccessAction,
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

  $renameRole = createEffect(() =>
    this.actions$.pipe(
      ofType(renameRoleAction),
      switchMap((request) => {
        return this.rolesService.renameRole(request.role).pipe(
          map(() => renameRoleSuccessAction({role: request.role})),
          catchError((response: HttpErrorResponse) =>
            of(
              renameRoleFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $renameRoleSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(renameRoleSuccessAction),
        tap(() => {
          this.toasts.showSuccess(`Role successfully renamed`);
        })
      ),
    {dispatch: false}
  );

  $moveUpRole = createEffect(() =>
    this.actions$.pipe(
      ofType(moveUpRoleAction),
      switchMap((request) => {
        return this.rolesService.moveUpRole(request).pipe(
          map((response) => moveUpRoleSuccessAction({roles: response})),
          catchError((response: HttpErrorResponse) =>
            of(
              moveUpRoleFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $moveDownRole = createEffect(() =>
    this.actions$.pipe(
      ofType(moveDownRoleAction),
      switchMap((request) => {
        return this.rolesService.moveDownRole(request).pipe(
          map((response) => moveDownRoleSuccessAction({roles: response})),
          catchError((response: HttpErrorResponse) =>
            of(
              moveDownRoleFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private rolesService: RolesService,
    private toasts: ToastService
  ) {}
}
