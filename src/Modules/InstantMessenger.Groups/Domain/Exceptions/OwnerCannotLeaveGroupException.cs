namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class OwnerCannotLeaveGroupException : DomainException
    {
        public override string Code => "owner_cannot_leave";

        public OwnerCannotLeaveGroupException() : base($"Owner cannot leave the group.")
        {
        }
    }
}