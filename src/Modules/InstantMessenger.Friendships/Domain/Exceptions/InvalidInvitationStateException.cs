namespace InstantMessenger.Friendships.Domain.Exceptions
{
    public class InvalidInvitationStateException : DomainException
    {
        public override string Code => "invalid_invitation_state";

        public InvalidInvitationStateException() : base("Invitation state cannot be changed.")
        {
        }
    }
}