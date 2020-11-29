import {createAction, props} from '@ngrx/store';

import {SignUpRequestInterface} from 'src/app/identity/types/signUpRequest.interface';
import {ActionTypes} from 'src/app/identity/store/actionTypes';
import {ErrorResponseInterface} from '../../../shared/types/error.response';

export const signUpAction = createAction(
  ActionTypes.SIGN_UP,
  props<{request: SignUpRequestInterface}>()
);

export const signUpSuccessAction = createAction(ActionTypes.SIGN_UP_SUCCESS);

export const signUpFailureAction = createAction(
  ActionTypes.SIGN_UP_FAILURE,
  props<{error: ErrorResponseInterface}>()
);
