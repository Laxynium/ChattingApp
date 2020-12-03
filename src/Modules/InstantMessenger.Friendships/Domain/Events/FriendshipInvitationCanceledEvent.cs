using System;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Friendships.Domain.Events
{
    [ProccessAlsoInternally]
    public class FriendshipInvitationCanceledEvent : IEvent
    {
        public Guid InvitationId { get; }
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipInvitationCanceledEvent(Guid invitationId, Guid senderId, Guid receiverId, DateTimeOffset createdAt)
        {
            InvitationId = invitationId;
            SenderId = senderId;
            ReceiverId = receiverId;
            CreatedAt = createdAt;
        }
    }
}