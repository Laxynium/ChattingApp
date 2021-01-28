import {Set} from 'immutable';
export interface AllowedAction {
  name: string;
  isChannelSpecific: boolean;
  channels: Set<string>;
}
