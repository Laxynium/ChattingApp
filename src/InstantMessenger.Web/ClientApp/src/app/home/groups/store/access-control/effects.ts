import {HttpErrorResponse} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {catchError, map, switchMap} from 'rxjs/operators';
import {GroupsService} from 'src/app/home/groups/services/groups.service';
import {
  getAllowedActionsAction,
  getAllowedActionsFailureAction,
  getAllowedActionsSuccessAction,
} from 'src/app/home/groups/store/access-control/actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class AccessControlEffects {
  $getAllowedActions = createEffect(() =>
    this.actions$.pipe(
      ofType(getAllowedActionsAction),
      switchMap(({groupId}) => {
        return this.groupsService.getAllowedActions(groupId).pipe(
          map((r) => getAllowedActionsSuccessAction({allowedActions: r})),
          catchError((response: HttpErrorResponse) =>
            of(
              getAllowedActionsFailureAction(),
              requestFailedAction({error: mapToError(response)})
            )
          )
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private groupsService: GroupsService
  ) {}
}
