using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Invitations.GenerateInvitationCode
{
    public class GenerateInvitationCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid InvitationId { get; }
        public ExpirationTimeCommandItem ExpirationTime { get; }
        public UsageCounterCommandItem UsageCounter { get; }

        public GenerateInvitationCommand(Guid userId, Guid groupId, Guid invitationId, ExpirationTimeCommandItem expirationTime, UsageCounterCommandItem usageCounter)
        {
            UserId = userId;
            GroupId = groupId;
            InvitationId = invitationId;
            ExpirationTime = expirationTime;
            UsageCounter = usageCounter;
        }

    }

    public enum ExpirationTimeTypeCommandItem
    {
        Infinite,
        Bounded
    }

    public class ExpirationTimeCommandItem
    {
        public ExpirationTimeTypeCommandItem Type { get; }
        public TimeSpan? Period { get; }

        public ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem type, TimeSpan? period = null)
        {
            Type = type;
            Period = period;
        }
    }

    public enum UsageCounterTypeCommandItem
    {
        Infinite, Bounded
    }

    public class UsageCounterCommandItem
    {
        public UsageCounterTypeCommandItem Type { get; }
        public int? Times { get; }

        public UsageCounterCommandItem(UsageCounterTypeCommandItem type, int? times = null)
        {
            Type = type;
            Times = times;
        }
    }
}