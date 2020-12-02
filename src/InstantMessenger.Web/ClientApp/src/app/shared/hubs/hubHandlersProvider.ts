import {Store} from '@ngrx/store';

export interface HubMethod<T> {
  (store: Store, data: T): void;
}

export interface HubHandlersProvider {
  (): {[methodName: string]: HubMethod<any>};
}

export type Hub = {
  hubName: string;
  hubHandlersProvider: HubHandlersProvider;
};
