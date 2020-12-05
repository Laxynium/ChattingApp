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
    public class PermissionDto
    {
        public string Name { get; }
        public int Code { get; }

        public PermissionDto(string name, int code)
        {
            Name = name;
            Code = code;
        }
    }
    public class GetRolePermissionsQuery : IQuery<IEnumerable<PermissionDto>>
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }

        public GetRolePermissionsQuery(Guid groupId, Guid roleId)
        {
            GroupId = groupId;
            RoleId = roleId;
        }
    }
    public class GetRolePermissionsHandler : IQueryHandler<GetRolePermissionsQuery, IEnumerable<PermissionDto>>
    {
        private readonly GroupsContext _context;

        public GetRolePermissionsHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PermissionDto>> HandleAsync(GetRolePermissionsQuery query)
        {
            var permissions = await _context.Groups.AsNoTracking()
                .Where(x => x.Id == GroupId.From(query.GroupId))
                .SelectMany(x => x.Roles)
                .Where(x => x.Id == RoleId.From(query.RoleId))
                .Select(x => x.Permissions)
                .ToListAsync();
            if(permissions.Count == 0)
                return new List<PermissionDto>();
            var permission = permissions[0];
            return permission.ToListOfPermissions()
                .Select(x => new PermissionDto(x.Name, x.Value))
                .ToList();
        }
    }
}