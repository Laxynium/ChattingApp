using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public class GetGroupChannelQuery : IQuery<ChannelDto>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }

        public GetGroupChannelQuery(Guid userId, Guid groupId, Guid channelId)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
        }
    }
    public sealed class GetGroupChannelHandler : IQueryHandler<GetGroupChannelQuery, ChannelDto>
    {
        private readonly GroupsContext _context;

        public GetGroupChannelHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<ChannelDto> HandleAsync(GetGroupChannelQuery query) => await _context.Channels.AsNoTracking()
            .Where(x => _context.Groups.Where(g => g.Id == query.GroupId).SelectMany(g => g.Members).Select(m => m.UserId).Any(u => u == UserId.From(query.UserId)))
            .Where(x => x.GroupId == GroupId.From(query.GroupId))
            .Where(x => x.Id == ChannelId.From(query.ChannelId))
            .Select(
                x => new ChannelDto
                {
                    ChannelId = x.Id.Value,
                    GroupId = x.GroupId.Value,
                    ChannelName = x.Name.Value
                }
            ).FirstOrDefaultAsync();
    }

}