using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.ResponseDtos;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public class GetInvitationsQuery : IQuery<IEnumerable<InvitationDto>>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }

        public GetInvitationsQuery(Guid userId, Guid groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }
    public class GetInvitationsHandler : IQueryHandler<GetInvitationsQuery, IEnumerable<InvitationDto>>
    {
        private readonly GroupsContext _context;

        public GetInvitationsHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<InvitationDto>> HandleAsync(GetInvitationsQuery query)
        {
            var group = await _context.Groups.AsNoTracking().SingleOrDefaultAsync(x => x.Id == GroupId.From(query.GroupId));
            if (!group?.CanAccessInvitations(UserId.From(query.UserId)) ?? false)
            {
                return new List<InvitationDto>();
            }
            var result = await _context.Invitations
                .Where(x => _context.Groups.Where(g => g.Id == GroupId.From(query.GroupId)).SelectMany(g => g.Members).Any(m => m.UserId == UserId.From(query.UserId)))
                .Where(x => x.GroupId == GroupId.From(query.GroupId))
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
                        Type = (UsageCounterTypeDto) (int) x.UsageCounterType,
                        Value = x.UsageCounter
                    }
                }
            ).ToList();
        }
    }

}