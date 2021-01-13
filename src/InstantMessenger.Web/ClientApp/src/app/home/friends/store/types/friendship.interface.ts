import {UserInterface} from 'src/app/shared/types/user.interface';

export interface FriendshipInterface {
  id: string;
  friend: UserInterface;
  createdAt: string;
}
