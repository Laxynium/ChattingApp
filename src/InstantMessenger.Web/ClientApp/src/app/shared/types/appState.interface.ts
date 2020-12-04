import {ConversationsStateInterface} from 'src/app/home/conversations/store/reducers';
import {FriendsStateInterface} from 'src/app/home/friends/store/reducers';
import {IdentityStateInterface} from 'src/app/identity/types/identityState.interface';

export interface AppStateInterface {
  identity: IdentityStateInterface;
  friends: FriendsStateInterface;
  conversations: ConversationsStateInterface;
}
