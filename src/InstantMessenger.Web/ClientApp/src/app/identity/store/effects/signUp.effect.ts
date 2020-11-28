import {Injectable} from '@angular/core';
import {Actions, createEffect, Effect, ofType} from '@ngrx/effects';
import {
  signUpAction,
  signUpFailureAction,
  signUpSuccessAction,
} from 'src/app/identity/store/actions/signUp.actions';
import {catchError, map, mergeMap, switchMap} from 'rxjs/operators';
import {IdentityService} from 'src/app/identity/services/identity.service';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';
import {of} from 'rxjs';

@Injectable()
export class SignUpEffect {
  signUp$ = createEffect(() =>
    this.actions$.pipe(
      ofType(signUpAction),
      switchMap(({request}) =>
        this.identityService.signUp(request).pipe(
          map((currentUser: CurrentUserInterface) =>
            signUpSuccessAction({currentUser})
          ),
          catchError((_error) => of(signUpFailureAction()))
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private identityService: IdentityService
  ) {}
}
