namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class OwnerCannotKickHimselfException : DomainException
    {
        public override string Code => "owner_cannot_kick_himself";

        public OwnerCannotKickHimselfException() : base($"Owner cannot kick himself from group.")
        {
        }
    }
}