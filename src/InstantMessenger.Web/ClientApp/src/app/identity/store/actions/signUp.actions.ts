import {createAction, props} from '@ngrx/store';

import {SignUpRequestInterface} from 'src/app/identity/store/types/signUpRequest.interface';
import {ActionTypes} from 'src/app/identity/store/actionTypes';

export const signUpAction = createAction(
  ActionTypes.SIGN_UP,
  props<{request: SignUpRequestInterface}>()
);

export const signUpSuccessAction = createAction(ActionTypes.SIGN_UP_SUCCESS);

export const signUpFailureAction = createAction(ActionTypes.SIGN_UP_FAILURE);
