using System;

namespace InstantMessenger.PrivateMessages.Application.Features.SendMessage
{
    public class SendMessageApiRequest
    {
        public Guid ConversationId { get; }
        public string Text { get; }

        public SendMessageApiRequest(Guid conversationId, string text)
        {
            ConversationId = conversationId;
            Text = text;
        }
    }
}