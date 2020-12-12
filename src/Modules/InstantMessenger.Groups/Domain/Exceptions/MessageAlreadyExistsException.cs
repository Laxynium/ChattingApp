using System;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    internal class MessageAlreadyExistsException : DomainException
    {
        public override string Code => "message_already_exists";
        public Guid MessageId { get; }

        public MessageAlreadyExistsException(Guid messageId):base($"Message[id={messageId}] already exists.")
        {
            MessageId = messageId;
        }
    }
}