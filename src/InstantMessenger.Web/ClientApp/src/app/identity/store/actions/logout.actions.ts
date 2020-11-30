import {createAction} from '@ngrx/store';
import {ActionTypes} from 'src/app/identity/store/actionTypes';

export const logoutActiion = createAction(ActionTypes.LOGOUT);

export const logoutSuccessAction = createAction(ActionTypes.LOGOUT_SUCCESS);

export const logoutFailureAction = createAction(ActionTypes.LOGOUT_FAILURE);
