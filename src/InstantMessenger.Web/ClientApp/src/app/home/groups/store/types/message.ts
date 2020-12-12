export interface ChannelMessageDto {
  channelId: string;
  groupId: string;
  message: MessageDto[];
}
export interface MessageDto {
  messageId: string;
  groupId: string;
  channelId: string;
  senderId: string;
  senderName: string;
  senderAvatar: string;
  content: string;
  createdAt: string;
}
