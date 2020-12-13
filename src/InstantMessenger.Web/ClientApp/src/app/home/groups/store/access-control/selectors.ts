import {createSelector} from '@ngrx/store';
import {Map} from 'immutable';
import {GroupsStateInterface} from 'src/app/home/groups/store/reducers';
import {groupsFeatureSelector} from 'src/app/home/groups/store/selectors';
import {AllowedAction} from 'src/app/home/groups/store/types/allowed-action';

export const allowedActionsSelector = createSelector(
  groupsFeatureSelector,
  (s: GroupsStateInterface): Map<string, AllowedAction> => s.allowedActions
);
