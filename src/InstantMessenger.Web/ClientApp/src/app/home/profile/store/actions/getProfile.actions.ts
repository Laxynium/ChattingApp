import {createAction, props} from '@ngrx/store';
import {ActionTypes} from 'src/app/home/profile/store/actionTypes';
import {Profile} from 'src/app/home/profile/types/profile';

export const getProfile = createAction(ActionTypes.UPLOAD_AVATAR);
export const getProfileSuccess = createAction(
  ActionTypes.UPLOAD_AVATAR_SUCCESS,
  props<{profile: Profile}>()
);
