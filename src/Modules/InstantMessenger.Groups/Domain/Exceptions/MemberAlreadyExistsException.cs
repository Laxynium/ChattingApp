using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class MemberAlreadyExistsException : DomainException
    {
        public UserId UserId { get; }
        public override string Code => "member_already_exists";

        public MemberAlreadyExistsException(UserId userId) : base($"Member for user[id={userId} already exists in group.")
        {
            UserId = userId;
        }
    }
}