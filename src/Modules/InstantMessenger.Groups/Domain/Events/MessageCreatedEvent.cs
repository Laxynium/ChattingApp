using System;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain.Events
{
    public class MessageCreatedEvent : IDomainEvent
    {
        public MessageId MessageId { get; }
        public GroupId GroupId { get; }
        public ChannelId ChannelId { get; }
        public UserId SenderId { get; }
        public MessageContent Content { get; }
        public DateTimeOffset CreatedAt { get; }

        public MessageCreatedEvent(MessageId messageId, GroupId groupId, ChannelId channelId,
            UserId senderId, MessageContent content, DateTimeOffset createdAt)
        {
            MessageId = messageId;
            GroupId = groupId;
            ChannelId = channelId;
            SenderId = senderId;
            Content = content;
            CreatedAt = createdAt;
        }
    }
}