using System;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.PrivateMessages.Api.IntegrationEvents
{
    public class MessageCreatedIntegrationEvent : IIntegrationEvent
    {
        public Guid MessageId { get; }
        public Guid ConversationId { get; }
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }
        public string Content { get; }
        public DateTimeOffset CreatedAt { get; }

        public MessageCreatedIntegrationEvent(Guid messageId, Guid conversationId, Guid senderId, Guid receiverId, string content, DateTimeOffset createdAt)
        {
            MessageId = messageId;
            ConversationId = conversationId;
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            CreatedAt = createdAt;
        }
    }
}