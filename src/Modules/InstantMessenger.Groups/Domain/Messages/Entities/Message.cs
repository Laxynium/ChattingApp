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
        public GroupId GroupId { get; }
        public ChannelId ChannelId { get; }
        public MessageContent Content { get; }

        public DateTimeOffset CreatedAt { get; }

        public Message(MessageId id, UserId from, GroupId groupId, ChannelId channelId, MessageContent content, DateTimeOffset createdAt):base(id)
        {
            From = from;
            GroupId = groupId;
            ChannelId = channelId;
            Content = content;
            CreatedAt = createdAt;
        }
    }
}