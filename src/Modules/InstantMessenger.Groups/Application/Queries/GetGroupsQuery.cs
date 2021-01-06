using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.ResponseDtos;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Application.Queries
{
    public class GetGroupsQuery : IQuery<IEnumerable<GroupDto>>
    {
        public Guid UserId { get; }
        public Guid? GroupId { get; }

        public GetGroupsQuery(Guid userId, Guid? groupId = null)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }

    public class GetGroupsHandler : IQueryHandler<GetGroupsQuery, IEnumerable<GroupDto>>
    {
        private readonly GroupsContext _context;

        public GetGroupsHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GroupDto>> HandleAsync(GetGroupsQuery query)
        {
            var groups = _context.Groups.AsNoTracking()
                .Include(x=>x.Members)
                .Where(x=>x.Members.Select(m=>m.UserId).Any(x=>x == UserId.From(query.UserId)));
            if (query.GroupId.HasValue)
            {
                groups = groups.Where(x => x.Id == GroupId.From(query.GroupId.Value));
            }

            return await groups.Select(
                x => new GroupDto
                {
                    GroupId = x.Id.Value,
                    Name = x.Name.Value,
                    CreatedAt = x.CreatedAt,
                }
            ).ToListAsync();
        }
    }
}