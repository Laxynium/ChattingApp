using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public class RoleDto
    {
        public Guid RoleId { get; }
        public string Name { get; }
        public int Priority { get; }

        public RoleDto(Guid roleId, string name, int priority)
        {
            RoleId = roleId;
            Name = name;
            Priority = priority;
        }
    }

    public class GetMemberRolesQuery : IQuery<IEnumerable<RoleDto>>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid UserIdOfMember { get; }

        public GetMemberRolesQuery(Guid userId, Guid groupId, Guid userIdOfMember)
        {
            UserId = userId;
            GroupId = groupId;
            UserIdOfMember = userIdOfMember;
        }
    }

    public class GetMemberRolesQueryHandler : IQueryHandler<GetMemberRolesQuery, IEnumerable<RoleDto>>
    {
        private readonly GroupsContext _context;

        public GetMemberRolesQueryHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RoleDto>> HandleAsync(GetMemberRolesQuery query)
        {
            var memberRoleIds = _context.Groups
                .AsNoTracking()
                .Where(x => x.Id == GroupId.From(query.GroupId))
                .SelectMany(x => x.Members)
                .Where(x => x.UserId == UserId.From(query.UserIdOfMember))
                .SelectMany(x => x.Roles)
                .Select(x=>x.Value);

            return await _context.Groups.AsNoTracking()
                .Where(x => x.Id == GroupId.From(query.GroupId))
                .SelectMany(x => x.Roles)
                .Where(x => memberRoleIds.Contains(x.Id.Value))
                .OrderByDescending(x => x.Priority.Value)
                .Select(x=>new RoleDto(x.Id.Value, x.Name.Value, x.Priority.Value))
                .ToListAsync();
        }
    }
}