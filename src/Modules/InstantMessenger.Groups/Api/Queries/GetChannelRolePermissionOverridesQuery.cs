using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public enum OverrideTypeDto
    {
        Allow,
        Deny
    }
    public class RolePermissionOverrideDto
    {
        public string Permission { get; set; }
        public OverrideTypeDto Type { get; set; }
    }
    public class GetChannelRolePermissionOverridesQuery : IQuery<IEnumerable<RolePermissionOverrideDto>>
    {
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid RoleId { get; }

        public GetChannelRolePermissionOverridesQuery(Guid groupId, Guid channelId, Guid roleId)
        {
            GroupId = groupId;
            ChannelId = channelId;
            RoleId = roleId;
        }
    }
    public class GetChannelRolePermissionOverridesHandler : IQueryHandler<GetChannelRolePermissionOverridesQuery, IEnumerable<RolePermissionOverrideDto>>
    {
        private readonly GroupsContext _context;

        public GetChannelRolePermissionOverridesHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RolePermissionOverrideDto>> HandleAsync(GetChannelRolePermissionOverridesQuery query)
        {
            return await _context.Channels
                .AsNoTracking()
                .Where(x => x.GroupId == GroupId.From(query.GroupId))
                .Where(x => x.Id == ChannelId.From(query.ChannelId))
                .SelectMany(x => x.RolePermissionOverrides)
                .Where(x => x.RoleId == RoleId.From(query.RoleId))
                .Select(
                    x => new RolePermissionOverrideDto
                    {
                        Permission = x.Permission.Name,
                        Type = (OverrideTypeDto) (int) x.Type
                    }
                ).ToListAsync();
        }
    }
}