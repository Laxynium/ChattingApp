using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.ResponseDtos;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Queries;

namespace InstantMessenger.Groups.Application.Queries
{
    public class GetAvailablePermissionsQuery : IQuery<IEnumerable<PermissionDto>>
    {
        public Guid GroupId { get; }

        public GetAvailablePermissionsQuery(Guid groupId)
        {
            GroupId = groupId;
        }
    }

    public sealed class GetAvailablePermissionsQueryHandler: IQueryHandler<GetAvailablePermissionsQuery,IEnumerable<PermissionDto>>
    {
        public GetAvailablePermissionsQueryHandler()
        {
            
        }
        public Task<IEnumerable<PermissionDto>> HandleAsync(GetAvailablePermissionsQuery query)
        {
            return Task.FromResult(Permission.List.Select(p => new PermissionDto(p.Name, p.Value)));
        }
    }
}