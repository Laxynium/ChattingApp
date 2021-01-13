import {UserInterface} from 'src/app/shared/types/user.interface';

export interface InvitationInterface {
  invitationId: string;
  senderId: string;
  receiverId: string;
  status: string;
  createdAt: string;
  type: string;
}

export interface InvitationFullInterface {
  invitationId: string;
  sender: UserInterface;
  receiver: UserInterface;
  status: string;
  createdAt: string;
  type: string;
  isLoading: boolean;
}
