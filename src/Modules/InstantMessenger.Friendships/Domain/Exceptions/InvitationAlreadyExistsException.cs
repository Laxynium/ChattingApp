namespace InstantMessenger.Friendships.Domain.Exceptions
{
    public sealed class InvitationAlreadyExistsException : DomainException
    {
        public override string Code => "invitation_already_exists";

        public InvitationAlreadyExistsException() : base("Invitation already exists.")
        {
        }
    }
}