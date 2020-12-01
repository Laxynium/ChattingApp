namespace InstantMessenger.PrivateMessages.Domain.Exceptions
{
    public class MessageNotFoundException : DomainException
    {
        public override string Code => "message_not_found";

        public MessageNotFoundException() : base("Message was not found")
        {
        }
    }
}