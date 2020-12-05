using System;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.PrivateMessages.Domain.Events
{
    public class MessageMarkedAsReadDomainEvent: IDomainEvent
    {
        public MessageId MessageId { get; }
        public ConversationId ConversationId { get; }
        public Participant Sender { get; }
        public Participant Receiver { get; }

        public DateTimeOffset ReadAt { get; }

        public MessageMarkedAsReadDomainEvent(MessageId messageId, ConversationId conversationId, Participant sender, Participant receiver, DateTimeOffset readAt)
        {
            MessageId = messageId;
            ConversationId = conversationId;
            ReadAt = readAt;
            Sender = sender;
            Receiver = receiver;
        }
    }
}