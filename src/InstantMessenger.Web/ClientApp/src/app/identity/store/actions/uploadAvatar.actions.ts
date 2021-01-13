import {createAction, props} from '@ngrx/store';
import {UploadAvatarRequest} from 'src/app/identity/store/types/uploadAvatar.request';
import {ActionTypes} from 'src/app/identity/store/actionTypes';
import {CurrentUserInterface} from 'src/app/shared/types/currentUser.interface';

export const uploadAvatar = createAction(
  ActionTypes.UPLOAD_AVATAR,
  props<{request: UploadAvatarRequest}>()
);
export const uploadAvatarSuccess = createAction(
  ActionTypes.UPLOAD_AVATAR_SUCCESS,
  props<{user: CurrentUserInterface}>()
);
