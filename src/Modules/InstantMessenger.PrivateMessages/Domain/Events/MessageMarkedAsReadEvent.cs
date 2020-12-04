using System;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.PrivateMessages.Domain.Events
{
    [ProccessAlsoInternally]
    public class MessageMarkedAsReadEvent: IEvent
    {
        public Guid MessageId { get; }
        public Guid ConversationId { get; }
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }

        public DateTimeOffset ReadAt { get; }

        public MessageMarkedAsReadEvent(Guid messageId, Guid conversationId, Guid senderId, Guid receiverId, DateTimeOffset readAt)
        {
            MessageId = messageId;
            ConversationId = conversationId;
            ReadAt = readAt;
            SenderId = senderId;
            ReceiverId = receiverId;
        }
    }
}