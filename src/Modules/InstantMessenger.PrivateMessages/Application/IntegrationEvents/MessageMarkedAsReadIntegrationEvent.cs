using System;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.PrivateMessages.Application.IntegrationEvents
{
    public class MessageMarkedAsReadIntegrationEvent: IIntegrationEvent
    {
        public Guid MessageId { get; }
        public Guid ConversationId { get; }
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }

        public DateTimeOffset ReadAt { get; }

        public MessageMarkedAsReadIntegrationEvent(Guid messageId, Guid conversationId, Guid senderId, Guid receiverId, DateTimeOffset readAt)
        {
            MessageId = messageId;
            ConversationId = conversationId;
            ReadAt = readAt;
            SenderId = senderId;
            ReceiverId = receiverId;
        }
    }
}