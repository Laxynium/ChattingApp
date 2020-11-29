import {Action, ActionReducer, MetaReducer} from '@ngrx/store';
import {mergeDeepRight, mergeRight, pick} from 'ramda';
import {PersistanceService} from 'src/app/shared/services/persistance.service';
import {currentUser} from './selectors';
import {CurrentUserInterface} from '../../shared/types/currentUser.interface';
import {AppStateInterface} from '../../shared/types/appState.interface';

export function storageMetaReducer<S, A extends Action = Action>(
  saveKeys: string[],
  localStorageKey: string,
  storageService: PersistanceService
) {
  let onInit = true;
  return function (reducer: ActionReducer<S, A>) {
    return function (state: S, action: A): S {
      const nextState = reducer(state, action);
      const currentUser = storageService.get('currentUser');
      if (currentUser && state) {
        const merged: any = mergeDeepRight(<any>state, {
          currentUser: currentUser,
        });
        return merged;
      }
      //   if (onInit) {
      //     onInit = false;
      //     const savedState = storageService.get(localStorageKey);
      //     return mergeDeepRight(<any>nextState, savedState);
      //   }
      //   const stateToSave: any = pick(saveKeys, nextState);
      //   storageService.set(stateToSave, localStorageKey);

      return nextState;
    };
  };
}

export function getMetaReducers(): MetaReducer<any>[] {
  const service = new PersistanceService();
  return [
    storageMetaReducer(
      ['id', 'nickname', 'email', 'token'],
      'identity.currentUser',
      service
    ),
  ];
}
