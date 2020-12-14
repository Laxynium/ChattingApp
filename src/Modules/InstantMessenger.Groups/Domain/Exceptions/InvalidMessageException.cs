namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvalidMessageException : DomainException
    {
        public override string Code => "invalid_message";

        public InvalidMessageException(string value) : base($"'{value}' is invalid message.")
        {
        }
    }
}