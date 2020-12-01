namespace InstantMessenger.Friendships.Domain.Exceptions
{
    public sealed class InvalidInvitationException : DomainException
    {
        public override string Code => "invalid_invitation";

        public InvalidInvitationException() : base("Invitation is invalid.")
        {
        }
    }
}