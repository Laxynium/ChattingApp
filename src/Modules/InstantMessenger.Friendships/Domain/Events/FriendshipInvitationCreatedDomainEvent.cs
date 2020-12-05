using System;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Friendships.Domain.Events
{
    public class FriendshipInvitationCreatedDomainEvent : IDomainEvent
    {
        public InvitationId InvitationId { get; }
        public PersonId SenderId { get; }
        public PersonId ReceiverId { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipInvitationCreatedDomainEvent(InvitationId invitationId, PersonId senderId, PersonId receiverId, DateTimeOffset createdAt)
        {
            InvitationId = invitationId;
            SenderId = senderId;
            ReceiverId = receiverId;
            CreatedAt = createdAt;
        }
    }
}