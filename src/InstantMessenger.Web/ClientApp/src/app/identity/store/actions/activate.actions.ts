import {createAction, props} from '@ngrx/store';
import { ActionTypes } from 'src/app/identity/store/actionTypes';
import { ActivateRequestInterface } from 'src/app/identity/types/activateRequest.interface';

export const activateAction = createAction(
  ActionTypes.ACTIVATE,
  props<{request: ActivateRequestInterface}>()
);
export const activateSuccessAction = createAction(ActionTypes.ACTIVATE_SUCCESS);

export const activateFailureAction = createAction(ActionTypes.ACTIVATE_FAILURE);
