export interface ChannelMessageDto {
  channelId: string;
  groupId: string;
  message: MessageDto[];
}
export interface MessageDto {
  channelId: string;
  groupId: string;
  messageId: string;
  senderId: string;
  senderName: string;
  senderAvatar: string;
  content: string;
  createdAt: string;
}
