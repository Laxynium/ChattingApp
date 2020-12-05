using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.PrivateMessages.Api.Hubs
{
    public class MessageDto
    {
        public Guid MessageId { get; }
        public Guid ConversationId { get; }
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }
        public string Content { get; }
        public DateTimeOffset CreatedAt { get; }

        public MessageDto(Guid messageId, Guid conversationId, Guid senderId, Guid receiverId, string content, DateTimeOffset createdAt)
        {
            MessageId = messageId;
            ConversationId = conversationId;
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            CreatedAt = createdAt;
        }
    }

    public class MessageMarkedAsReadDto
    {
        public Guid MessageId { get; }
        public Guid ConversationId { get; }
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }

        public DateTimeOffset ReadAt { get; }

        public MessageMarkedAsReadDto(Guid messageId, Guid conversationId, Guid senderId, Guid receiverId, DateTimeOffset readAt)
        {
            MessageId = messageId;
            ConversationId = conversationId;
            ReadAt = readAt;
            SenderId = senderId;
            ReceiverId = receiverId;
        }
    }

    public class ConversationDto
    {
        public Guid ConversationId { get; }
        public Guid FirstParticipantId { get; }
        public Guid SecondParticipantId { get; }

        public ConversationDto(Guid conversationId, Guid firstParticipantId, Guid secondParticipantId)
        {
            ConversationId = conversationId;
            FirstParticipantId = firstParticipantId;
            SecondParticipantId = secondParticipantId;
        }
    }

    public interface IPrivateMessagesHubContract
    {
        Task OnMessageCreated(MessageDto message);
        Task OnMessageRead(MessageMarkedAsReadDto message);
        Task OnConversationRemoved(ConversationDto conversation);
    }

    [Authorize]
    public class PrivateMessagesHub : Hub<IPrivateMessagesHubContract>
    {
        
    }
}