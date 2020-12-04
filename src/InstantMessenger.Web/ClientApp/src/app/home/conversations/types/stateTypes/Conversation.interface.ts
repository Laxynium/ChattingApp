import {UserInterface} from 'src/app/shared/types/user.interface';

export type LatestConversationInterface = ConversationInterface & {
  unreadCount: number;
};

export interface ConversationInterface {
  id: string;
  firstParticipant: ConversationParticipantInterface;
  secondParticipant: ConversationParticipantInterface;
  messages: ConversationMessageInterface[];
}
export type ConversationParticipantInterface = UserInterface;

export type ConversationMessageInterface = {
  id: string;
  conversationId: string;
  fromUserId: string;
  toUserId: string;
  createdAt: string;
  readAt: string;
} & TextContent;

export interface TextContent {
  body: string;
}
