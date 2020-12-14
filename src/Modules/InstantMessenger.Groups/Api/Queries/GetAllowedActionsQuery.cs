using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public class GetAllowedActionsQuery : IQuery<IEnumerable<AllowedActions>>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }

        public GetAllowedActionsQuery(Guid userId, Guid groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }

    public class GetAllowedActionsQueryHandler : IQueryHandler<GetAllowedActionsQuery, IEnumerable<AllowedActions>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly GroupsContext _context;

        public GetAllowedActionsQueryHandler(IGroupRepository groupRepository, GroupsContext context)
        {
            _groupRepository = groupRepository;
            _context = context;
        }
        public async Task<IEnumerable<AllowedActions>> HandleAsync(GetAllowedActionsQuery query)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(query.GroupId));
            if(group is null)
                return new List<AllowedActions>();

            var channels = await _context.Channels
                .AsNoTracking()
                .Where(g => g.GroupId == GroupId.From(query.GroupId))
                .ToListAsync();

            var allowedActions = group.GetAllowedActions(UserId.From(query.UserId), channels);

            return allowedActions;
        }
    }
}