using System;

namespace InstantMessenger.Groups.Api.ResponseDtos
{
    public class ChannelDto
    {
        public Guid ChannelId { get; set; }
        public Guid GroupId { get; set; }
        public string ChannelName { get; set; }

        public ChannelDto(Guid channelId, Guid groupId, string channelName)
        {
            ChannelId = channelId;
            GroupId = groupId;
            ChannelName = channelName;
        }
        public ChannelDto(){}
    }
}