import {createAction, props} from '@ngrx/store';
import {ActionTypes} from 'src/app/home/profile/store/actionTypes';
import {Profile} from 'src/app/home/profile/types/profile';

export const getProfile = createAction(ActionTypes.GET_PROFILE);
export const getProfileSuccess = createAction(
  ActionTypes.GET_PROFILE_SUCCESS,
  props<{profile: Profile}>()
);
