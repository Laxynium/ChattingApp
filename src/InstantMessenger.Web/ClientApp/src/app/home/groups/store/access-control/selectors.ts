import {createSelector} from '@ngrx/store';
import {Map} from 'immutable';
import {AllowedAction} from 'src/app/home/groups/store/types/allowed-action';
import {allowedActionsStateSelector} from '../selectors';
import {
  allowedActionAdapter,
  AllowedActionsState,
} from './reducer';

const selectors = allowedActionAdapter.getSelectors();

export const allowedActionsSelector = createSelector(
  allowedActionsStateSelector,
  (s: AllowedActionsState): Map<string, AllowedAction> =>
    Map(selectors.selectEntities(s))
);
