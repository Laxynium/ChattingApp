using System;

namespace InstantMessenger.Groups.Application.Features.Messages.SendMessage
{
    public class SendMessageApiRequest
    {
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid MessageId { get; }
        public string Content { get; }

        public SendMessageApiRequest(Guid groupId, Guid channelId, Guid messageId, string content)
        {
            GroupId = groupId;
            ChannelId = channelId;
            MessageId = messageId;
            Content = content;
        }
    }
}