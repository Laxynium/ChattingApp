import {HttpErrorResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {from, of} from 'rxjs';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {GroupsService} from 'src/app/home/groups/services/groups.service';
import {
  createGroupAction,
  createGroupSuccessAction,
  createGroupFailureAction,
  joinGroupAction,
  joinGroupSuccessAction,
  joinGroupFailureAction,
  getGroupsAction,
  removeGroupAction,
  removeGroupSuccessAction,
  removeGroupFailureAction,
  renameGroupAction,
  renameGroupSuccessAction,
  renameGroupFailureAction,
  getGroupsSuccessAction,
  getGroupsFailureAction,
  changeCurrentGroupAction,
  loadCurrentGroupAction,
  loadCurrentGroupSuccessAction,
  loadCurrentGroupFailureAction,
} from 'src/app/home/groups/store/groups/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {ToastService} from 'src/app/shared/toasts/toast.service';
import {mapToError} from 'src/app/shared/types/error.response';
import {v4 as guid} from 'uuid';

@Injectable()
export class GroupsEffects {
  $createGroup = createEffect(() =>
    this.actions$.pipe(
      ofType(createGroupAction),
      switchMap(({groupName}) => {
        const request = {
          groupId: guid(),
          groupName: groupName,
        };
        return this.groupsService.createGroup(request).pipe(
          map((r) => createGroupSuccessAction({group: r})),
          catchError((response: HttpErrorResponse) =>
            of(
              createGroupFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $joinGroup = createEffect(() =>
    this.actions$.pipe(
      ofType(joinGroupAction),
      switchMap((r) => {
        return this.groupsService.joinGroup(r).pipe(
          map((_) => joinGroupSuccessAction()),
          catchError((response) =>
            of(
              joinGroupFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $joinGroupSuccess = createEffect(() =>
    this.actions$.pipe(
      ofType(joinGroupSuccessAction),
      switchMap((r) => {
        return of(getGroupsAction());
      })
    )
  );

  $removeGroup = createEffect(() =>
    this.actions$.pipe(
      ofType(removeGroupAction),
      switchMap((request) => {
        return this.groupsService.removeGroup(request).pipe(
          map((r) => removeGroupSuccessAction({groupId: request.groupId})),
          catchError((response) =>
            of(
              removeGroupFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $renameGroup = createEffect(() =>
    this.actions$.pipe(
      ofType(renameGroupAction),
      switchMap((request) => {
        return this.groupsService.renameGroup(request.group).pipe(
          map(() => renameGroupSuccessAction({group: request.group})),
          catchError((response: HttpErrorResponse) =>
            of(
              renameGroupFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  $renameGroupSuccess = createEffect(
    () =>
      this.actions$.pipe(
        ofType(renameGroupSuccessAction),
        tap(() => {
          this.toasts.showSuccess(`Channel successfully renamed`);
        })
      ),
    {dispatch: false}
  );

  $getGroups = createEffect(() =>
    this.actions$.pipe(
      ofType(getGroupsAction),
      switchMap((_) => {
        return this.groupsService.getGroups().pipe(
          map((r) => getGroupsSuccessAction({groups: r})),
          catchError((response) =>
            of(
              getGroupsFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );
  $changeGroup = createEffect(
    () =>
      this.actions$.pipe(
        ofType(changeCurrentGroupAction),
        tap(({groupId}) => this.router.navigateByUrl(`/groups/${groupId}`))
      ),
    {dispatch: false}
  );

  $loadGroup = createEffect(() =>
    this.actions$.pipe(
      ofType(loadCurrentGroupAction),
      switchMap((r) => {
        return this.groupsService.getGroup(r.groupId).pipe(
          map((r) => loadCurrentGroupSuccessAction({group: r})),
          catchError((response) =>
            of(
              loadCurrentGroupFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );
  constructor(
    private actions$: Actions,
    private groupsService: GroupsService,
    private router: Router,
    private toasts: ToastService
  ) {}
}
