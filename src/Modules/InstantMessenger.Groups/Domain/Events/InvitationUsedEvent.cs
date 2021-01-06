using System;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain.Events
{
    public class InvitationUsedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid InvitationId { get; }
        public Guid GroupId { get; }

        public InvitationUsedEvent(Guid userId, Guid invitationId, Guid groupId)
        {
            UserId = userId;
            InvitationId = invitationId;
            GroupId = groupId;
        }
    }
}