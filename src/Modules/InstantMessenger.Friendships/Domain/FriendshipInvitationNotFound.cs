using System;
using InstantMessenger.Friendships.Domain.Exceptions;

namespace InstantMessenger.Friendships.Domain
{
    internal class FriendshipInvitationNotFound : DomainException
    {
        public override string Code => "invitation_not_found";

        public FriendshipInvitationNotFound() : base("Invitation was not found.")
        {
        }
    }
}