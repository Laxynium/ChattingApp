using System;

namespace InstantMessenger.Friendships.Api.Features.AcceptInvitation
{
    public class AcceptFriendshipInvitationApiRequest
    {
        public Guid InvitationId { get; }

        public AcceptFriendshipInvitationApiRequest(Guid invitationId)
        {
            InvitationId = invitationId;
        }
    }
}