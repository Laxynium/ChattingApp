namespace InstantMessenger.PrivateMessages.Domain.Exceptions
{
    public class ConversationNotFoundException: DomainException
    {
        public override string Code => "conversation_not_found";

        public ConversationNotFoundException() : base("Conversation was not found")
        {
        }
    }
}