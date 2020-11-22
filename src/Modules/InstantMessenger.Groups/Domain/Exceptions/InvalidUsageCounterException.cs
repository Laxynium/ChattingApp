namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvalidUsageCounterException : DomainException
    {
        public override string Code => "invalid_usage_counter";

        public InvalidUsageCounterException() : base($"Usage counter is invalid.")
        {
        }
    }
}