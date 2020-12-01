import {UserInterface} from 'src/app/home/friends/types/user.interface';

export interface FriendshipInterface {
  id: string;
  friend: UserInterface;
  createdAt: string;
}
