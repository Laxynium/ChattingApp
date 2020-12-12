using System;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class ChannelAlreadyExistsException : DomainException
    {
        public override string Code => "channel_already_exists";
        public Guid ChannelId { get; }

        public ChannelAlreadyExistsException(Guid channelId):base($"Channel[id={channelId}] already exists.")
        {
            ChannelId = channelId;
        }
    }
}