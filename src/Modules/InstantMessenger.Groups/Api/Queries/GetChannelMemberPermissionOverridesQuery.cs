using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.ResponseDtos;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public class GetChannelMemberPermissionOverridesQuery : IQuery<IEnumerable<PermissionOverrideDto>>
    {
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid MemberUserId { get; }

        public GetChannelMemberPermissionOverridesQuery(Guid groupId, Guid channelId, Guid memberUserId)
        {
            GroupId = groupId;
            ChannelId = channelId;
            MemberUserId = memberUserId;
        }
    }
    public class GetChannelMemberPermissionOverridesHandler : IQueryHandler<GetChannelMemberPermissionOverridesQuery, IEnumerable<PermissionOverrideDto>>
    {
        private readonly GroupsContext _context;

        public GetChannelMemberPermissionOverridesHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PermissionOverrideDto>> HandleAsync(GetChannelMemberPermissionOverridesQuery query)
        {
            var channels = await _context.Channels.ToListAsync();
            return await _context.Channels
                .AsNoTracking()
                .Where(x => x.GroupId == GroupId.From(query.GroupId))
                .Where(x => x.Id == ChannelId.From(query.ChannelId))
                .SelectMany(x => x.MemberPermissionOverrides)
                .Where(x => x.UserIdOfMember == UserId.From(query.MemberUserId))
                .Select(
                    x => new PermissionOverrideDto
                    {
                        Permission = x.Permission.Name,
                        Type = (OverrideTypeDto)(int)x.Type
                    }
                ).ToListAsync();
        }
    }

}