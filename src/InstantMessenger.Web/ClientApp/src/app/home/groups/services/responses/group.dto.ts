export interface GroupDto {
  groupId: string;
  name: string;
  createdAt: string;
  ownerId: string;
}

export interface ChannelDto {
  channelId: string;
  groupId: string;
  channelName: string;
}
