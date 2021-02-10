export type GroupId = string;
export interface GroupDto {
  groupId: GroupId;
  name: string;
  createdAt: string;
  ownerId: string;
}

export interface ChannelDto {
  channelId: string;
  groupId: string;
  channelName: string;
}
