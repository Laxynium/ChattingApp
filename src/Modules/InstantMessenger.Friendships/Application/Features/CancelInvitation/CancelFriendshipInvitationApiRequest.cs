using System;

namespace InstantMessenger.Friendships.Application.Features.CancelInvitation
{
    public class CancelFriendshipInvitationApiRequest
    {
        public Guid InvitationId { get; }

        public CancelFriendshipInvitationApiRequest(Guid invitationId)
        {
            InvitationId = invitationId;
        }
    }
}