import {ProfilesState} from 'src/app/home/profile/types/ProfilesState.interface';
import {IdentityStateInterface} from 'src/app/identity/types/identityState.interface';

export interface AppStateInterface {
  identity: IdentityStateInterface;
  profiles: ProfilesState;
}
