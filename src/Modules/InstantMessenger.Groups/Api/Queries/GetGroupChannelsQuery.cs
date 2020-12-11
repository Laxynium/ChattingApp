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

        public async Task<IEnumerable<ChannelDto>> HandleAsync(GetGroupChannelsQuery query)
        {
            var group = await _context.Groups.AsNoTracking()
                .Where(g => g.Id == GroupId.From(query.GroupId))
                .Where(g => g.Members.Select(m => m.UserId).Any(id => id == UserId.From(query.UserId)))
                .FirstOrDefaultAsync();
            if(group is null)
                return new List<ChannelDto>();

            var channels = await _context.Channels.AsNoTracking()
                .Where(
                    x => _context.Groups.Where(g => g.Id == x.GroupId)
                        .SelectMany(g => g.Members)
                        .Select(m => m.UserId)
                        .Any(u => u == UserId.From(query.UserId))
                )
                .Where(x => x.GroupId == GroupId.From(query.GroupId))
                .ToListAsync();

            return channels.Where(c => group.CanAccessChannel(UserId.From(query.UserId), c))
                .Select(
                    x => new ChannelDto
                    {
                        ChannelId = x.Id.Value,
                        GroupId = x.GroupId.Value,
                        ChannelName = x.Name.Value
                    }
                ).ToList();
        }
    }
}