using System;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class ChannelNotFoundException : DomainException
    {
        public Guid ChannelId { get; }
        public override string Code => "channel_not_found";

        public ChannelNotFoundException(Guid channelId) : base($"Channel[id={channelId}] was not found.")
        {
            ChannelId = channelId;
        }
    }
}