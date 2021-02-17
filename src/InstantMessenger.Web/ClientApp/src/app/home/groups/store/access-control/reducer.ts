import {Set} from 'immutable';
import {createEntityAdapter, EntityState} from '@ngrx/entity';
import {createReducer, on} from '@ngrx/store';
import {getAllowedActionsAction, getAllowedActionsFailureAction, getAllowedActionsSuccessAction} from "./actions";


export interface AllowedAction {
  name: string;
  isChannelSpecific: boolean;
  channels: Set<string>;
}

export const allowedActionAdapter = createEntityAdapter<AllowedAction>({
  selectId: (x) => x.name,
});

export interface AllowedActionsState extends EntityState<AllowedAction> {
  isLoading: boolean;
}

export const allowedActionsReducer = createReducer(
  allowedActionAdapter.getInitialState({isLoading: false}),
  on(getAllowedActionsAction, (s) => ({...s, isLoading: true})),
  on(getAllowedActionsSuccessAction, (state, action) => ({
    ...allowedActionAdapter.setAll(action.allowedActions, state),
    isLoading: false,
  })),
  on(getAllowedActionsFailureAction, (s) => ({...s, isLoading: false}))
);
