import {FriendsStateInterface} from 'src/app/home/friends/store/reducers';
import {ProfilesState} from 'src/app/home/profile/types/ProfilesState.interface';
import {IdentityStateInterface} from 'src/app/identity/types/identityState.interface';

export interface AppStateInterface {
  identity: IdentityStateInterface;
  profiles: ProfilesState;
  friends: FriendsStateInterface;
}
