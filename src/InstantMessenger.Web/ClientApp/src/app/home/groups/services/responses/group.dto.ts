export interface GroupDto {
  groupId: string;
  name: string;
  createdAt: string;
}

export interface ChannelDto {
  channelId: string;
  groupId: string;
  channelName: string;
}
