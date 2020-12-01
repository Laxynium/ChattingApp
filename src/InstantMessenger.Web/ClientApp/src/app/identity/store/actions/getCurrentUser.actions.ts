import {createAction, props} from '@ngrx/store';
import {ActionTypes} from 'src/app/identity/store/actionTypes';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';

export const getCurrentUser = createAction(ActionTypes.GET_CURRENT_USER);
export const getCurrentUserSuccess = createAction(
  ActionTypes.GET_CURRENT_USER_SUCCESS,
  props<{user: CurrentUserInterface}>()
);
