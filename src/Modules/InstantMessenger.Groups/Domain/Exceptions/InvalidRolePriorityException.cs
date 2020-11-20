namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvalidRolePriorityException : DomainException
    {
        public int Value { get; }
        public override string Code => "invalid_role_priority";

        public InvalidRolePriorityException(int value) : base($"'{value}' is invalid role priority.")
        {
            Value = value;
        }
    }
}