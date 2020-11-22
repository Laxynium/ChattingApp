namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvalidInvitationException : DomainException
    {
        public override string Code => "invalid_invitation";

        public InvalidInvitationException() : base("Invitation is invalid.")
        {
        }
    }
}