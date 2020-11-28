import {createAction, props} from '@ngrx/store';

import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';
import {SignUpRequestInterface} from 'src/app/identity/types/signUpRequest.interface';
import {ActionTypes} from 'src/app/identity/store/actionTypes';

export const signUpAction = createAction(
  ActionTypes.REGISTER,
  props<{request: SignUpRequestInterface}>()
);

export const signUpSuccessAction = createAction(
  ActionTypes.REGISTER_SUCCESS,
  props<{currentUser: CurrentUserInterface}>()
);

export const signUpFailureAction = createAction(ActionTypes.REGISTER_FAILURE);
