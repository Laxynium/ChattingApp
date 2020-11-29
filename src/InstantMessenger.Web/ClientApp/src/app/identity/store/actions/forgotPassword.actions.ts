import {createAction, props} from '@ngrx/store';
import {ActionTypes} from 'src/app/identity/store/actionTypes';
import {ResetPasswordRequestInterface} from 'src/app/identity/types/resetPasswordRequest.interface';
import {ErrorResponseInterface} from 'src/app/shared/types/error.response';
import {ForgotPasswordRequestInterface} from '../../types/forgotPasswordRequest.interface';

export const forgotPasswordAction = createAction(
  ActionTypes.FORGOT_PASSWORD,
  props<{request: ForgotPasswordRequestInterface}>()
);

export const forgotPasswordSuccessAction = createAction(
  ActionTypes.FORGOT_PASSWORD_SUCCESS
);

export const forgotPasswordFailureAction = createAction(
  ActionTypes.FORGOT_PASSWORD_FAILURE
);

export const resetPasswordAction = createAction(
  ActionTypes.RESET_PASSWORD,
  props<{request: ResetPasswordRequestInterface}>()
);

export const resetPasswordSuccessAction = createAction(
  ActionTypes.RESET_PASSWORD_SUCCESS
);

export const resetPasswordFailureAction = createAction(
  ActionTypes.RESET_PASSWORD_FAILURE,
  props<{error: ErrorResponseInterface}>()
);
