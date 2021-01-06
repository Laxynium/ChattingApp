using System;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.Friendships.Api.IntegrationEvents
{
    public class FriendshipInvitationAcceptedIntegrationEvent : IIntegrationEvent
    {
        public Guid InvitationId { get; }
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipInvitationAcceptedIntegrationEvent(Guid invitationId, Guid senderId, Guid receiverId, DateTimeOffset createdAt)
        {
            InvitationId = invitationId;
            SenderId = senderId;
            ReceiverId = receiverId;
            CreatedAt = createdAt;
        }
    }
}