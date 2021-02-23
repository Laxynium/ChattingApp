import {Action, ActionReducer, MetaReducer} from '@ngrx/store';
import {mergeDeepRight} from 'ramda';
import {PersistanceService} from 'src/app/shared/services/persistance.service';

export function storageMetaReducer<S, A extends Action = Action>(
  storageService: PersistanceService
) {
  let onInit = true;
  return function (reducer: ActionReducer<S, A>) {
    return function (state: S, action: A): S {
      const nextState = reducer(state, action);
      if (onInit) {
        onInit = false;
        const currentUser = storageService.get('currentUser');
        const merged: any = mergeDeepRight(<any>nextState, {
          currentUser: currentUser || null,
        });
        return merged;
      }
      return nextState;
    };
  };
}

export function getMetaReducers(): MetaReducer<any>[] {
  const service = new PersistanceService();
  return [storageMetaReducer(service)];
}
