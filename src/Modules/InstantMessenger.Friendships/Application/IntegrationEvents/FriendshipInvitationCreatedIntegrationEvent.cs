﻿using System;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.Friendships.Application.IntegrationEvents
{
    public class FriendshipInvitationCreatedIntegrationEvent : IIntegrationEvent
    {
        public Guid InvitationId { get; }
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipInvitationCreatedIntegrationEvent(Guid invitationId, Guid senderId, Guid receiverId, DateTimeOffset createdAt)
        {
            InvitationId = invitationId;
            SenderId = senderId;
            ReceiverId = receiverId;
            CreatedAt = createdAt;
        }
    }
}