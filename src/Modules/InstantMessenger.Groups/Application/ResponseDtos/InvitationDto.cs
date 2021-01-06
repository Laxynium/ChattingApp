using System;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Application.ResponseDtos
{
    public class InvitationDto
    {
        public Guid GroupId { get; set; }
        public Guid InvitationId { get; set; }
        public string Code { get; set; }
        public ExpirationTimeDto ExpirationTime { get; set; }
        public UsageCounterDto UsageCounter { get; set; }
        public InvitationDto(Guid groupId, Guid invitationId, string code, ExpirationTimeDto expirationTime, UsageCounterDto usageCounter)
        {
            GroupId = groupId;
            InvitationId = invitationId;
            Code = code;
            ExpirationTime = expirationTime;
            UsageCounter = usageCounter;
        }
        public InvitationDto(){}

    }
    public enum ExpirationTimeTypeDto
    {
        Infinite, Bounded
    }
    public class ExpirationTimeDto
    {
        public ExpirationTimeTypeDto Type { get; set; }
        public DateTimeOffset Start { get; set; }
        public TimeSpan? Period { get; set; }

        public ExpirationTimeDto(ExpirationTimeTypeDto type, DateTimeOffset start, TimeSpan? period)
        {
            Type = type;
            Start = start;
            Period = period;
        }
        public ExpirationTimeDto(){}
    }
    public enum UsageCounterTypeDto
    {
        Infinite, Bounded
    }
    public class UsageCounterDto
    {
        public UsageCounterTypeDto Type { get; set; }
        public int? Value { get; set; }

        public UsageCounterDto(UsageCounterTypeDto type, int? value)
        {
            Type = type;
            Value = value;
        }
        public UsageCounterDto(){}
    }

    public static class Extensions
    {
        public static ExpirationTimeDto ToDto(this ExpirationTime expirationTime) =>
            expirationTime switch
        {
            InfiniteExpirationTime x=>new ExpirationTimeDto(ExpirationTimeTypeDto.Infinite, x.Start, null),
            BoundedExpirationTime x=>new ExpirationTimeDto(ExpirationTimeTypeDto.Bounded, x.Start, x.Period),
        };
        public static UsageCounterDto ToDto(this UsageCounter usageCounter) =>
            usageCounter switch
        {
            InfiniteUsageCounter x=> new UsageCounterDto(UsageCounterTypeDto.Infinite, null),
            BoundedUsageCounter x=> new UsageCounterDto(UsageCounterTypeDto.Bounded, x.Value),
        };
    }
}