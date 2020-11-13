using System;

namespace InstantMessenger.PrivateMessages.Api.Features.MarkMessageAsRead
{
    public class MarkMessageAsReadApiRequest
    {
        public Guid MessageId { get; }

        public MarkMessageAsReadApiRequest(Guid messageId)
        {
            MessageId = messageId;
        }
    }
}