using System;

namespace InstantMessenger.Groups.Api.Features.Invitations.GenerateInvitationCode
{
    public class GenerateInvitationApiRequest
    {
        public Guid GroupId { get; }
        public Guid InvitationId { get; }
        public ExpirationTimeCommandItem ExpirationTime { get; }
        public UsageCounterCommandItem UsageCounter { get; }

        public GenerateInvitationApiRequest(Guid groupId, Guid invitationId, ExpirationTimeCommandItem expirationTime, UsageCounterCommandItem usageCounter)
        {
            GroupId = groupId;
            InvitationId = invitationId;
            ExpirationTime = expirationTime;
            UsageCounter = usageCounter;
        }

    }
}