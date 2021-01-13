import {UserInterface} from 'src/app/shared/types/user.interface';

export interface ConversationResponseInterface {
  conversationId: string;
  firstParticipant: UserInterface;
  secondParticipant: UserInterface;
  messages: MessageResponseInterface[];
  unreadMessagesCount: number | null;
}
export interface MessageResponseInterface {
  messageId: string;
  from: string;
  to: string;
  content: string;
  createdAt: string;
  readAt: string;
}
