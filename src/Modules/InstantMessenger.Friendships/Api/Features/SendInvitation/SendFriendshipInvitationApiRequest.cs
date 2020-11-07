using System;

namespace InstantMessenger.Friendships.Api.Features.SendInvitation
{
    public class SendFriendshipInvitationApiRequest
    {
        public Guid ReceiverId { get; }

        public SendFriendshipInvitationApiRequest(Guid receiverId)
        {
            ReceiverId = receiverId;
        }
    }
}