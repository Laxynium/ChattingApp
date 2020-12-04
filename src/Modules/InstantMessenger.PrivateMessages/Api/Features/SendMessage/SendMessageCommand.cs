using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.PrivateMessages.Api.Features.SendMessage
{
    public class SendMessageCommand : ICommand
    {
        public Guid MessageId { get; }
        public Guid ConversationId { get; }
        public Guid SenderId { get; }
        public string Text { get; }

        public SendMessageCommand(Guid messageId, Guid conversationId, Guid senderId, string text)
        {
            MessageId = messageId;
            ConversationId = conversationId;
            SenderId = senderId;
            Text = text;
        }
    }
}