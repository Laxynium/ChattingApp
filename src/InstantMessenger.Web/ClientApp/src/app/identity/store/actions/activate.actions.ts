import {createAction, props} from '@ngrx/store';

import {ActionTypes} from '../actionTypes';
import {ActivateRequestInterface} from '../../types/activateRequest.interface';

export const activateAction = createAction(
  ActionTypes.ACTIVATE,
  props<{request: ActivateRequestInterface}>()
);
export const activateSuccessAction = createAction(ActionTypes.ACTIVATE_SUCCESS);

export const activateFailureAction = createAction(ActionTypes.ACTIVATE_FAILURE);
