using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain.Events
{
    public class InvitationCreatedEvent : IDomainEvent
    {
        public InvitationId InvitationId { get; }
        public GroupId GroupId { get; }
        public InvitationCode InvitationCode { get; }
        public ExpirationTime ExpirationTime { get; }
        public UsageCounter UsageCounter { get; }
        public InvitationCreatedEvent(InvitationId invitationId, GroupId groupId, InvitationCode invitationCode, ExpirationTime expirationTime, UsageCounter usageCounter)
        {
            InvitationId = invitationId;
            GroupId = groupId;
            InvitationCode = invitationCode;
            ExpirationTime = expirationTime;
            UsageCounter = usageCounter;
        }
    }
}