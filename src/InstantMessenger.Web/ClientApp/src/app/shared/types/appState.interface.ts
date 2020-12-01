import {FriendsStateInterface} from 'src/app/home/friends/store/reducers';
import {IdentityStateInterface} from 'src/app/identity/types/identityState.interface';

export interface AppStateInterface {
  identity: IdentityStateInterface;
  friends: FriendsStateInterface;
}
