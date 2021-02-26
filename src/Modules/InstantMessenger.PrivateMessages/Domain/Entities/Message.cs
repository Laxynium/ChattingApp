using System;
using InstantMessenger.PrivateMessages.Domain.Events;
using InstantMessenger.PrivateMessages.Domain.Exceptions;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;
using NodaTime;

namespace InstantMessenger.PrivateMessages.Domain.Entities
{
    public class Message : Entity<MessageId>
    {
        public MessageBody Body { get; }
        public ConversationId ConversationId { get; }
        public DateTimeOffset CreatedAt { get; }
        public Participant From { get; }
        public Participant To { get; }
        public DateTimeOffset? ReadAt { private set; get; }
        private Message(){}

        private Message(MessageId id,
            MessageBody body,
            ConversationId conversationId,
            Participant @from,
            Participant to,
            DateTimeOffset createdAt,
            DateTimeOffset? readAt = null) : base(id)
        {
            Body = body;
            ConversationId = conversationId;
            CreatedAt = createdAt;
            To = to;
            From = @from;
            ReadAt = readAt;
            Apply(new MessageCreatedDomainEvent(Id, ConversationId, From, To, Body, CreatedAt));
        }

        public static Message Create(MessageId messageId, MessageBody body, ConversationId conversationId, Participant @from, Participant to, IClock clock) =>
            new(messageId, 
                body,
                conversationId,
                @from,
                to,
                clock.GetCurrentInstant()
                    .InUtc()
                    .ToDateTimeOffset());

        public void MarkAsRead(Participant participant, IClock clock)
        {
            if (participant != To)
            {
                throw new InvalidParticipantException();
            }

            ReadAt = clock.GetCurrentInstant().InUtc().ToDateTimeOffset();
            Apply(new MessageMarkedAsReadDomainEvent(Id, ConversationId, From, To, CreatedAt));
        }
    }
}