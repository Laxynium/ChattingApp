using System;

namespace InstantMessenger.Friendships.Application.Features.RejectInvitation
{
    public class RejectFriendshipInvitationApiRequest
    {
        public Guid InvitationId { get; }

        public RejectFriendshipInvitationApiRequest(Guid invitationId)
        {
            InvitationId = invitationId;
        }
    }
}