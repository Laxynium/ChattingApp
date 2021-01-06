using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.ResponseDtos;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Application.Queries
{
    public class GetRoleQuery : IQuery<RoleDto>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid RoleId { get; }

        public GetRoleQuery(Guid userId, Guid groupId, Guid roleId)
        {
            UserId = userId;
            GroupId = groupId;
            RoleId = roleId;
        }
    }

    public class GetRoleQueryHandler : IQueryHandler<GetRoleQuery, RoleDto>
    {
        private readonly GroupsContext _context;

        public GetRoleQueryHandler(GroupsContext context) => _context = context;

        public async Task<RoleDto> HandleAsync(GetRoleQuery query) => await _context.Groups.AsNoTracking()
            .Where(x => x.Id == GroupId.From(query.GroupId))
            .Where(x=>x.Members.Select(m=>m.UserId).Any(u=>u == UserId.From(query.UserId)))
            .SelectMany(x => x.Roles)
            .Where(x=>x.Id == RoleId.From(query.RoleId))
            .OrderByDescending(x => x.Priority)
            .Select(x => new RoleDto(query.GroupId,x.Id.Value, x.Name.Value, x.Priority.Value))
            .FirstOrDefaultAsync();
    }
}