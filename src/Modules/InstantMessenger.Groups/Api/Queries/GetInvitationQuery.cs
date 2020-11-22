using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public enum ExpirationTimeTypeDto
    {
        Infinite,Bounded
    }
    public class ExpirationTimeDto
    {
        public ExpirationTimeTypeDto Type { get; set; }
        public DateTimeOffset Start { get; set; }
        public TimeSpan? Period { get; set; }
    }
    public enum UsageCounterTypeDto
    {
        Infinite, Bounded
    }
    public class UsageCounterDto
    {
        public UsageCounterTypeDto Type { get; set; }
        public int? Value { get; set; }
    }
    public class InvitationDto
    {
        public Guid GroupId { get; set; }
        public Guid InvitationId { get; set; }
        public string Code { get; set; }
        public ExpirationTimeDto ExpirationTime { get; set; }
        public UsageCounterDto UsageCounter { get; set; }

    }
    public class GetInvitationQuery : IQuery<InvitationDto>
    {
        public Guid UserId { get; }
        public Guid InvitationId { get; }

        public GetInvitationQuery(Guid userId, Guid invitationId)
        {
            UserId = userId;
            InvitationId = invitationId;
        }
    }

    public class GetInvitationHandler : IQueryHandler<GetInvitationQuery,InvitationDto>
    {
        private readonly GroupsContext _context;

        public GetInvitationHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<InvitationDto> HandleAsync(GetInvitationQuery query)
        {
            var result = await _context.Invitations.Where(x => x.Id == InvitationId.From(query.InvitationId))
                .Select(
                    x => new
                    {
                        GroupId = x.GroupId.Value,
                        InvitationId = x.Id.Value,
                        Code = x.InvitationCode.Value,
                        ExpirationTimeType =
                            EF.Property<ExpirationTimeType>(EF.Property<ExpirationTimeContainer>(x, "_expirationTime"), "_type"),
                        ExpirationStart =
                            EF.Property<DateTimeOffset>(EF.Property<ExpirationTimeContainer>(x, "_expirationTime"), "_start"),
                        ExpirationPeriod =
                            EF.Property<TimeSpan?>(EF.Property<ExpirationTimeContainer>(x, "_expirationTime"), "_period"),
                        UsageCounterType =
                            EF.Property<UsageCounterType>(EF.Property<UsageCounterContainer>(x, "_usageCounter"), "_type"),
                        UsageCounter =
                            EF.Property<int?>(EF.Property<UsageCounterContainer>(x, "_usageCounter"), "_value"),
                    }
                ).ToListAsync();
            return result.Select(
                x => new InvitationDto
                {
                    GroupId = x.GroupId,
                    InvitationId = x.InvitationId,
                    Code = x.Code,
                    ExpirationTime = new ExpirationTimeDto
                    {
                        Type = (ExpirationTimeTypeDto) (int) x.ExpirationTimeType,
                        Start = x.ExpirationStart,
                        Period = x.ExpirationPeriod
                    },
                    UsageCounter = new UsageCounterDto
                    {
                        Type = (UsageCounterTypeDto) (int)x.UsageCounterType,
                        Value = x.UsageCounter
                    }
                }
            ).FirstOrDefault();
        }
    }
}