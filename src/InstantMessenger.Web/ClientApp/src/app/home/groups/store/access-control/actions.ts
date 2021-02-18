import {createAction, props} from '@ngrx/store';
import {AllowedAction} from "src/app/home/groups/store/access-control/reducer";

export enum ActionTypes {
  GET_ALLOWED_ACTIONS = '[Groups] Get allowed actions',
  GET_ALLOWED_ACTIONS_SUCCESS = '[Groups] Get allowed actions success',
  GET_ALLOWED_ACTIONS_FAILURE = '[Groups] Get allowed actions failure',
}

export const getAllowedActionsAction = createAction(
  ActionTypes.GET_ALLOWED_ACTIONS,
  props<{groupId: string}>()
);
export const getAllowedActionsSuccessAction = createAction(
  ActionTypes.GET_ALLOWED_ACTIONS_SUCCESS,
  props<{allowedActions: AllowedAction[]}>()
);
export const getAllowedActionsFailureAction = createAction(
  ActionTypes.GET_ALLOWED_ACTIONS_FAILURE
);
