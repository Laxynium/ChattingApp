import {Action, createReducer, on} from '@ngrx/store';

import {IdentityStateInterface} from '../types/identityState.interface';
import {
  forgotPasswordAction,
  forgotPasswordFailureAction,
  forgotPasswordSuccessAction,
} from './actions/forgotPassword.actions';
import {
  activateAction,
  activateSuccessAction,
  activateFailureAction,
} from './actions/activate.actions';
import {
  signInAction,
  signInSuccessAction,
  signInFailureAction,
} from './actions/signIn.actions';
import {
  signUpAction,
  signUpFailureAction,
  signUpSuccessAction,
} from './actions/signUp.actions';
import {
  logoutActiion,
  logoutSuccessAction,
} from 'src/app/identity/store/actions/logout.actions';
import {changeNicknameSuccessAction} from 'src/app/identity/store/actions/changeNickname.actions';

const initialState: IdentityStateInterface = {
  isSubmitting: false,
  currentUser: null,
};

const identityReducer = createReducer(
  initialState,
  on(
    signUpAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: true,
    })
  ),
  on(
    signUpSuccessAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: false,
    })
  ),
  on(
    signUpFailureAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: false,
    })
  ),

  on(
    activateAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: true,
    })
  ),
  on(
    activateSuccessAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: false,
    })
  ),
  on(
    activateFailureAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: false,
    })
  ),

  on(
    signInAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: true,
    })
  ),
  on(
    signInSuccessAction,
    (state, action): IdentityStateInterface => ({
      ...state,
      isSubmitting: false,
      currentUser: action.currentUser,
    })
  ),
  on(
    signInFailureAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: false,
    })
  ),

  on(
    forgotPasswordAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: true,
    })
  ),
  on(
    forgotPasswordSuccessAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: false,
    })
  ),
  on(
    forgotPasswordFailureAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: false,
    })
  ),

  on(
    logoutActiion,
    (state): IdentityStateInterface => ({
      ...state,
      currentUser: null,
    })
  ),
  on(
    logoutSuccessAction,
    (state): IdentityStateInterface => ({
      ...state,
      currentUser: null,
    })
  ),
  on(
    changeNicknameSuccessAction,
    (state, action): IdentityStateInterface => ({
      ...state,
      currentUser: {
        ...state.currentUser,
        nickname: action.response.nickname,
      },
    })
  )
);

export function reducers(state: IdentityStateInterface, action: Action) {
  return identityReducer(state, action);
}
