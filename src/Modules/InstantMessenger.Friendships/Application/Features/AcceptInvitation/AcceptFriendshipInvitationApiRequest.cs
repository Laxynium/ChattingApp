using System;

namespace InstantMessenger.Friendships.Application.Features.AcceptInvitation
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