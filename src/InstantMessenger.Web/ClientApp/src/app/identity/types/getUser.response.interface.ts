import {Avatar} from 'src/app/identity/types/avatar';

export interface GetUserResponseInterface {
  id: string;
  nickname: string;
  email: string;
  avatar: Avatar;
}
