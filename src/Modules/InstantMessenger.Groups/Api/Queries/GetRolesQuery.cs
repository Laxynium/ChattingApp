using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public class GetRolesQuery : IQuery<IEnumerable<RoleDto>>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }

        public GetRolesQuery(Guid userId, Guid groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }
    public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, IEnumerable<RoleDto>>
    {
        private readonly GroupsContext _context;

        public GetRolesQueryHandler(GroupsContext context) => _context = context;

        public async Task<IEnumerable<RoleDto>> HandleAsync(GetRolesQuery query) => await _context.Groups.AsNoTracking()
            .Where(x=>x.Id == GroupId.From(query.GroupId))
            .SelectMany(x=>x.Roles)
            .OrderByDescending(x => x.Priority)
            .Select(x => new RoleDto(x.Id.Value, x.Name.Value, x.Priority.Value))
            .ToListAsync();
    }
}