using System;

namespace InstantMessenger.Groups.Api.Features.Channel.EditChannel
{
    public class RenameChannelApiRequest
    {
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public string Name { get; }

        public RenameChannelApiRequest(Guid groupId, Guid channelId, string name)
        {
            GroupId = groupId;
            ChannelId = channelId;
            Name = name;
        }
    }
}