namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvitationNotFoundException : DomainException
    {
        public override string Code => "invitation_not_found";

        public InvitationNotFoundException() : base("Invitation was not found.")
        {
        }
    }
}