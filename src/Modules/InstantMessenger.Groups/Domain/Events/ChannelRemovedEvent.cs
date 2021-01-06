using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain.Events
{
    public class ChannelRemovedEvent : IDomainEvent
    {
        public ChannelId ChannelId { get; }
        public GroupId GroupId { get; }
        public ChannelName ChannelName { get; }

        public ChannelRemovedEvent(ChannelId channelId, GroupId groupId, ChannelName channelName)
        {
            ChannelId = channelId;
            GroupId = groupId;
            ChannelName = channelName;
        }
    }
}