using InstantMessenger.Friendships.Domain.Exceptions;

namespace InstantMessenger.Friendships.Api
{
    public sealed class InvitationAlreadyExistsException : DomainException
    {
        public override string Code => "invitation_already_exists";

        public InvitationAlreadyExistsException() : base("Invitation already exists.")
        {
        }
    }
}