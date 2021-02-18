import {ConversationsStateInterface} from 'src/app/home/conversations/store/reducers';
import {FriendsStateInterface} from 'src/app/home/friends/store/reducers';
import {IdentityStateInterface} from 'src/app/identity/store/types/identityState.interface';
import {GroupsModuleState} from "src/app/home/groups/store/reducers";

export interface AppStateInterface {
  identity: IdentityStateInterface;
  friends: FriendsStateInterface;
  conversations: ConversationsStateInterface;
  groups: GroupsModuleState;
}
