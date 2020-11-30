import {createAction, props} from '@ngrx/store';
import {ActionTypes} from 'src/app/home/profile/store/actionTypes';
import {Avatar} from 'src/app/home/profile/types/avatar';
import {Profile} from 'src/app/home/profile/types/profile';
import {UploadAvatarRequest} from 'src/app/home/profile/types/uploadAvatar.request';
import {ErrorResponseInterface} from 'src/app/shared/types/error.response';

export const uploadAvatar = createAction(
  ActionTypes.UPLOAD_AVATAR,
  props<{request: UploadAvatarRequest}>()
);
export const uploadAvatarSuccess = createAction(
  ActionTypes.UPLOAD_AVATAR_SUCCESS,
  props<{profile: Profile}>()
);
