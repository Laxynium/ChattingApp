using System;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Messages.Entities
{
    public class Message : Entity<MessageId>
    {
        public UserId From { get; }
        public ChannelId ChannelId { get; }
        public MessageContent Content { get; }

        public DateTimeOffset CreatedAt { get; }

        public Message(MessageId id, UserId from, ChannelId channelId, MessageContent content, DateTimeOffset createdAt):base(id)
        {
            From = from;
            ChannelId = channelId;
            Content = content;
            CreatedAt = createdAt;
        }
    }
}