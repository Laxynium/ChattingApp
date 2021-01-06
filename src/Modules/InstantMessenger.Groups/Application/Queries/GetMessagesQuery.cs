using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Application.Queries
{
    public class GetMessagesQuery : IQuery<IEnumerable<GroupMessageView>>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }

        public GetMessagesQuery(Guid userId, Guid groupId, Guid channelId)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
        }
    }
    public class GetMessagesQueryHandler : IQueryHandler<GetMessagesQuery, IEnumerable<GroupMessageView>>
    {
        private readonly GroupsViewContext _viewContext;
        private readonly GroupsContext _groupsContext;

        public GetMessagesQueryHandler(GroupsViewContext viewContext, GroupsContext groupsContext)
        {
            _viewContext = viewContext;
            _groupsContext = groupsContext;
        }

        public async Task<IEnumerable<GroupMessageView>> HandleAsync(GetMessagesQuery query)
        {
            var group = await _groupsContext.Groups.AsNoTracking()
                .Where(g=>g.Members.Select(m=>m.UserId).Any(id=>id == UserId.From(query.UserId)))
                .FirstOrDefaultAsync(g => g.Id == GroupId.From(query.GroupId));
            var channel = await _groupsContext.Channels.AsNoTracking().FirstOrDefaultAsync(
                c => c.GroupId == GroupId.From(query.GroupId) && c.Id == ChannelId.From(query.ChannelId)
            );
            if(group is null || channel is null)
                return new List<GroupMessageView>();
            if (!group.CanAccessMessages(UserId.From(query.UserId), channel))
            {
                return new List<GroupMessageView>();
            }
            var groupMessageDtos = await _viewContext.GroupMessages
                .Where(m => m.GroupId == query.GroupId 
                            && m.ChannelId == query.ChannelId)
                .ToListAsync();
            return groupMessageDtos;
        }
    }
}