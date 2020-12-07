using System;

namespace InstantMessenger.Groups.Api.Features.Channel.AddChannel
{
    public class CreateChannelApiRequest
    {
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public string ChannelName { get; }

        public CreateChannelApiRequest(Guid groupId, Guid channelId, string channelName)
        {
            GroupId = groupId;
            ChannelId = channelId;
            ChannelName = channelName;
        }
    }
}