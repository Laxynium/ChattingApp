using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InsufficientPermissionsException : DomainException
    {
        public UserId UserId { get; }
        public override string Code => "insufficient_permissions";

        public InsufficientPermissionsException(UserId userId) : base($"User[id={userId}] has insufficient permissions.")
        {
            UserId = userId;
        }
    }
}