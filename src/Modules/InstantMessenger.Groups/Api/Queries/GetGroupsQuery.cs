using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public class GroupDto
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
    public class GetGroupsQuery : IQuery<IEnumerable<GroupDto>>
    {
        public Guid? GroupId { get; }

        public GetGroupsQuery(Guid? groupId = null)
        {
            GroupId = groupId;
        }
    }

    public class GetGroupHandler : IQueryHandler<GetGroupsQuery, IEnumerable<GroupDto>>
    {
        private readonly GroupsContext _context;

        public GetGroupHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GroupDto>> HandleAsync(GetGroupsQuery query)
        {
            var groups = _context.Groups.AsNoTracking();
            if (query.GroupId.HasValue)
            {
                groups = groups.Where(x => x.Id == GroupId.From(query.GroupId.Value));
            }

            return await groups.Select(
                x => new GroupDto
                {
                    GroupId = x.Id.Value,
                    Name = x.Name.Value,
                    CreatedAt = x.CreatedAt
                }
            ).ToListAsync();
        }
    }
}