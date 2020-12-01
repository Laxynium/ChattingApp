import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {of} from 'rxjs';
import {switchMap, catchError, map, tap} from 'rxjs/operators';
import {ProfileService} from 'src/app/home/profile/services/profile.service';
import {
  getProfile,
  getProfileSuccess,
} from 'src/app/home/profile/store/actions/getProfile.actions';
import {} from 'src/app/identity/store/actions/signIn.actions';
import {requestFailedAction} from 'src/app/shared/store/api-request.error';
import {mapToError} from 'src/app/shared/types/error.response';

@Injectable()
export class GetProfileEffect {
  $getProfile = createEffect(() =>
    this.actions$.pipe(
      ofType(getProfile),
      switchMap(() =>
        this.profilesService.getProfile().pipe(
          map((p) => getProfileSuccess({profile: p})),
          catchError((response) =>
            of(requestFailedAction({error: mapToError(response)}))
          )
        )
      )
    )
  );
  constructor(
    private actions$: Actions,
    private profilesService: ProfileService
  ) {}
}
