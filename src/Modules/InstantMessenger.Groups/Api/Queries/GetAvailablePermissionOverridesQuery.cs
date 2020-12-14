using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.ResponseDtos;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Shared.Messages.Queries;

namespace InstantMessenger.Groups.Api.Queries
{
    public class GetAvailablePermissionOverridesQuery : IQuery<IEnumerable<PermissionDto>>
    {
    }
    public class GetAvailablePermissionOverridesQueryHandler : IQueryHandler<GetAvailablePermissionOverridesQuery, IEnumerable<PermissionDto>>
    {
        public GetAvailablePermissionOverridesQueryHandler()
        {
            
        }
        public Task<IEnumerable<PermissionDto>> HandleAsync(GetAvailablePermissionOverridesQuery query)
        {
            var permissions = PermissionOverride.ValidPermissions;
            return Task.FromResult(permissions.Select(p => new PermissionDto(p.Name, p.Value)));

        }
    }
}