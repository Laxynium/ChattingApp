import {Avatar} from 'src/app/identity/types/avatar';

export interface CurrentUserInterface {
  id: string;
  nickname: string;
  email: string;
  token: string;
  avatar: Avatar;
}
