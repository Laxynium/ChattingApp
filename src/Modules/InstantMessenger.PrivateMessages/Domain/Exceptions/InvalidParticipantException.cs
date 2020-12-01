namespace InstantMessenger.PrivateMessages.Domain.Exceptions
{
    internal class InvalidParticipantException : DomainException
    {
        public override string Code => "invalid_participant";

        public InvalidParticipantException() : base("Conversation participant is invalid")
        {
        }
    }
}