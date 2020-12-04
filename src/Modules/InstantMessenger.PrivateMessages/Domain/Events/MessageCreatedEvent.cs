using System;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.PrivateMessages.Domain.Events
{
    [ProccessAlsoInternally]
    public class MessageCreatedEvent : IEvent
    {
        public Guid MessageId { get; }
        public Guid ConversationId { get; }
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }
        public string Content { get; }
        public DateTimeOffset CreatedAt { get; }

        public MessageCreatedEvent(Guid messageId, Guid conversationId, Guid senderId, Guid receiverId, string content, DateTimeOffset createdAt)
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