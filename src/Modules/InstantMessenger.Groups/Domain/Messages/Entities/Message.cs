using System;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Events;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Groups.Domain.Messages.Entities
{
    public sealed class Message : Entity<MessageId>
    {
        public GroupId GroupId { get; }
        public ChannelId ChannelId { get; }
        public UserId From { get; }
        public MessageContent Content { get; }

        public DateTimeOffset CreatedAt { get; }

        public Message(MessageId id, GroupId groupId, ChannelId channelId, UserId from, MessageContent content, DateTimeOffset createdAt):base(id)
        {
            ChannelId = channelId;
            From = from;
            Content = content;
            CreatedAt = createdAt;
            GroupId = groupId;
            Apply(new MessageCreatedEvent(Id,GroupId,ChannelId, From, Content,CreatedAt));
        }
    }
}