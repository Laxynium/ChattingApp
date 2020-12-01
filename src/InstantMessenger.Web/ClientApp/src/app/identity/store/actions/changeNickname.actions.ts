import {createAction, props} from '@ngrx/store';
import {ActionTypes} from 'src/app/identity/store/actionTypes';
import {ChangeNickname} from 'src/app/identity/types/changeNickname.request';

export const changeNicknameAction = createAction(
  ActionTypes.CHANGE_NICKNAME,
  props<{request: ChangeNickname}>()
);
export const changeNicknameSuccessAction = createAction(
  ActionTypes.CHANGE_NICKNAME_SUCCESS,
  props<{response: {nickname: string}}>()
);
