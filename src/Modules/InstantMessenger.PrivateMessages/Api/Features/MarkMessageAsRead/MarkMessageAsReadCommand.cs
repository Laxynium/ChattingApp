using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.PrivateMessages.Api.Features.MarkMessageAsRead
{
    public class MarkMessageAsReadCommand : ICommand
    {
        public Guid MessageId { get; }
        public Guid ParticipantId { get; }

        public MarkMessageAsReadCommand(Guid messageId, Guid participantId)
        {
            MessageId = messageId;
            ParticipantId = participantId;
        }
    }
}