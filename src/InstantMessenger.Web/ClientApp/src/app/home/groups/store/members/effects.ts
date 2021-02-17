import {HttpErrorResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {MembersService} from 'src/app/home/groups/services/members.service';
import {RolesService} from 'src/app/home/groups/services/roles.service';
import {
  addRoleToMemberAction,
  addRoleToMemberFailureAction,
  addRoleToMemberSuccessAction,
  getMemberRolesAction,
  getMemberRolesFailureAction,
  getMemberRolesSuccessAction,
  getMembersAction,
  getMembersFailureAction,
  getMembersSuccessAction,
  kickMemberAction,
  kickMemberFailureAction,
  kickMemberSuccessAction,
  removeRoleFromMemberAction,
  removeRoleFromMemberFailureAction,
  removeRoleFromMemberSuccessAction,
} from 'src/app/home/groups/store/members/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class MembersEffects {
  $getMembers = createEffect(() =>
    this.actions$.pipe(
      ofType(getMembersAction),
      switchMap((request) => {
        return this.membersService.getMembers(request).pipe(
          map((r) => getMembersSuccessAction({members: r})),
          catchError((response: HttpErrorResponse) =>
            of(
              getMembersFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $kickMember = createEffect(() =>
    this.actions$.pipe(
      ofType(kickMemberAction),
      switchMap((request) => {
        return this.membersService.kickMember(request).pipe(
          map(() => kickMemberSuccessAction(request)),
          catchError((response: HttpErrorResponse) =>
            of(
              kickMemberFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $kickMemberSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(kickMemberSuccessAction),
        tap(() => {
          this.toasts.showSuccess(`Member successfully kicked`);
        })
      ),
    {dispatch: false}
  );

  $getMemberRoles = createEffect(() =>
    this.actions$.pipe(
      ofType(getMemberRolesAction),
      switchMap((request) => {
        return this.membersService
          .getMemberRoles({userId: request.userId, groupId: request.groupId})
          .pipe(
            map((r) =>
              getMemberRolesSuccessAction({
                userId: request.userId,
                memberId: request.memberId,
                roles: r,
              })
            ),
            catchError((response: HttpErrorResponse) =>
              of(
                getMemberRolesFailureAction(),
                requestFailedAction({error: mapToError(response)})
              )
            )
          );
      })
    )
  );

  $addRoleToMember = createEffect(() =>
    this.actions$.pipe(
      ofType(addRoleToMemberAction),
      switchMap((request) => {
        return this.membersService.addRoleToMember(request.memberRole).pipe(
          map((_) => addRoleToMemberSuccessAction(request)),
          catchError((response) =>
            of(
              addRoleToMemberFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $addRoleToMemberSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(addRoleToMemberSuccessAction),
        tap(() => {
          this.toasts.showSuccess(`Role added to member successfully`);
        })
      ),
    {dispatch: false}
  );

  $removeRoleFromMember = createEffect(() =>
    this.actions$.pipe(
      ofType(removeRoleFromMemberAction),
      switchMap((request) => {
        return this.membersService
          .removeRoleFromMember(request.memberRole)
          .pipe(
            map((_) => removeRoleFromMemberSuccessAction(request)),
            catchError((response) =>
              of(
                removeRoleFromMemberFailureAction(),
                requestFailedAction({error: mapToError(response)})
              )
            )
          );
      })
    )
  );

  $removeRoleFromMemberSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(removeRoleFromMemberSuccessAction),
        tap(() => {
          this.toasts.showSuccess(`Role removed from member successfully`);
        })
      ),
    {dispatch: false}
  );

  constructor(
    private actions$: Actions,
    private membersService: MembersService,
    private toasts: ToastService
  ) {}
}
