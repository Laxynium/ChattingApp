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
    public class ChannelDto
    {
        public Guid ChannelId { get; set; }
        public Guid GroupId { get; set; }
        public string ChannelName { get; set; }
    }
    public class GetGroupChannelsQuery : IQuery<IEnumerable<ChannelDto>>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }

        public GetGroupChannelsQuery(Guid userId, Guid groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }

    public sealed class GetGroupChannelsHandler : IQueryHandler<GetGroupChannelsQuery, IEnumerable<ChannelDto>>
    {
        private readonly GroupsContext _context;

        public GetGroupChannelsHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ChannelDto>> HandleAsync(GetGroupChannelsQuery query) => await _context.Channels.AsNoTracking()
            .Where(x => x.GroupId == GroupId.From(query.GroupId))
            .Select(
                x => new ChannelDto
                {
                    ChannelId = x.Id.Value,
                    GroupId = x.GroupId.Value,
                    ChannelName = x.Name.Value
                }
            ).ToListAsync();
    }
}