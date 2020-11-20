using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class MemberNotFoundException : DomainException
    {
        public UserId UserId { get; }
        public override string Code => "member_not_found";

        public MemberNotFoundException(UserId userId) : base($"Member for user[id={userId} was not found.")
        {
            UserId = userId;
        }
    }
}