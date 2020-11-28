import {Action, createReducer, on} from '@ngrx/store';

import {IdentityStateInterface} from '../types/identityState.interface';
import {
  signUpAction,
  signUpFailureAction,
  signUpSuccessAction,
} from './actions/signUp.actions';

const initialState: IdentityStateInterface = {
  isSubmitting: false,
  activationCodeSend: false,
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
    (state, action): IdentityStateInterface => ({
      ...state,
      isSubmitting: false,
      activationCodeSend: true,
    })
  ),
  on(
    signUpFailureAction,
    (state): IdentityStateInterface => ({
      ...state,
      isSubmitting: false,
      activationCodeSend: false,
    })
  )
);

export function reducers(state: IdentityStateInterface, action: Action) {
  return identityReducer(state, action);
}
