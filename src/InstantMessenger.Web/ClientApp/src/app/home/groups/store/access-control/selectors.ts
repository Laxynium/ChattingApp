import {createSelector} from '@ngrx/store';
import {Map} from 'immutable';
import {allowedActionsStateSelector} from 'src/app/home/groups/store/selectors';
import {
  AllowedAction,
  allowedActionAdapter,
  AllowedActionsState,
} from 'src/app/home/groups/store/access-control/reducer';

const selectors = allowedActionAdapter.getSelectors();

export const allowedActionsSelector = createSelector(
  allowedActionsStateSelector,
  (s: AllowedActionsState): Map<string, AllowedAction> =>
    Map(selectors.selectEntities(s))
);
