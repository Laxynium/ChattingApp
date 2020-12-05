using System;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Shared.BuildingBlocks;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Identity.Domain.Events
{
    public class ActivationLinkCreatedDomainEvent: IDomainEvent
    {
        public ActivationLinkId Id { get; }
        public string Token { get; }
        public EntityId UserId { get; }
        public DateTimeOffset CreatedAt { get; }

        public ActivationLinkCreatedDomainEvent(ActivationLinkId id, string token, EntityId userId, DateTimeOffset createdAt)
        {
            Id = id;
            Token = token;
            UserId = userId;
            CreatedAt = createdAt;
        }
    }
}