import {Action, createReducer, on} from '@ngrx/store';
import {getProfileSuccess} from 'src/app/home/profile/store/actions/getProfile.actions';
import {uploadAvatarSuccess} from 'src/app/home/profile/store/actions/uploadAvatar.actions';
import {ProfilesState} from 'src/app/home/profile/types/ProfilesState.interface';

const initialState: ProfilesState = {
  avatar: null,
};

const profilesReducer = createReducer(
  initialState,
  on(
    uploadAvatarSuccess,
    (state, action): ProfilesState => ({
      ...state,
      avatar: action.profile.avatar,
    })
  ),
  on(
    getProfileSuccess,
    (state, action): ProfilesState => ({
      ...state,
      avatar: action.profile.avatar,
    })
  )
);

export function reducers(state: ProfilesState, action: Action) {
  return profilesReducer(state, action);
}
