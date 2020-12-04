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
    
    public interface IPrivateMessagesHubContract
    {
        Task OnMessageCreated(MessageDto message);
    }

    [Authorize]
    public class PrivateMessagesHub : Hub<IPrivateMessagesHubContract>
    {
        
    }
}