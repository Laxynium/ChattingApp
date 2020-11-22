using System;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.Groups.Domain
{
    public class InvitationUsedEvent : IEvent
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