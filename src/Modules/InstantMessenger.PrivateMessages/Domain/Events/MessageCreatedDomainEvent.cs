using System;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.PrivateMessages.Domain.Events
{
    public class MessageCreatedDomainEvent : IDomainEvent
    {
        public MessageId MessageId { get; }
        public ConversationId ConversationId { get; }
        public Participant Sender { get; }
        public Participant Receiver { get; }
        public MessageBody Content { get; }
        public DateTimeOffset CreatedAt { get; }

        public MessageCreatedDomainEvent(MessageId messageId, ConversationId conversationId, Participant sender, Participant receiver, MessageBody content, DateTimeOffset createdAt)
        {
            MessageId = messageId;
            ConversationId = conversationId;
            Sender = sender;
            Receiver = receiver;
            Content = content;
            CreatedAt = createdAt;
        }
    }
}