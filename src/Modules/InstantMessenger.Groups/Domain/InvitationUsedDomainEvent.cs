using System;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain
{
    public class InvitationUsedDomainEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid InvitationId { get; }
        public Guid GroupId { get; }

        public InvitationUsedDomainEvent(Guid userId, Guid invitationId, Guid groupId)
        {
            UserId = userId;
            InvitationId = invitationId;
            GroupId = groupId;
        }
    }
}