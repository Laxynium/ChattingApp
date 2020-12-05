namespace InstantMessenger.Friendships.Domain.Exceptions
{
    internal class FriendshipInvitationNotFound : DomainException
    {
        public override string Code => "invitation_not_found";

        public FriendshipInvitationNotFound() : base("Invitation was not found.")
        {
        }
    }
}