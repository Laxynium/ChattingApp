import {Avatar} from 'src/app/identity/store/types/avatar';

export interface GetUserResponseInterface {
  id: string;
  nickname: string;
  email: string;
  avatar: Avatar;
}
