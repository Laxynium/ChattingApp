import {createAction, props} from '@ngrx/store';

import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';
import {ActionTypes} from 'src/app/identity/store/actionTypes';
import {ErrorResponseInterface} from 'src/app/shared/types/error.response';
import {SignInRequestInterface} from 'src/app/identity/types/signInRequest.interface';

export const signInAction = createAction(
  ActionTypes.SIGN_IN,
  props<{request: SignInRequestInterface}>()
);

export const signInSuccessAction = createAction(
  ActionTypes.SIGN_IN_SUCCESS,
  props<{currentUser: CurrentUserInterface}>()
);

export const signInFailureAction = createAction(
  ActionTypes.SIGN_IN_FAILURE,
  props<{error: ErrorResponseInterface}>()
);
