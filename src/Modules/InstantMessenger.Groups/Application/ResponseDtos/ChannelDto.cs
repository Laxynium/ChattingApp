using System;

namespace InstantMessenger.Groups.Application.ResponseDtos
{
    public class ChannelDto
    {
        public Guid GroupId { get; set; }
        public Guid ChannelId { get; set; }
        public string ChannelName { get; set; }

        public ChannelDto(Guid groupId, Guid channelId, string channelName)
        {
            GroupId = groupId;
            ChannelId = channelId;
            ChannelName = channelName;
        }
        public ChannelDto(){}
    }
}