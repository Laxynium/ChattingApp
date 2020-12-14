using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
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
            
            var groupMessageDtos = await _viewContext.GroupMessages
                .Where(m => m.GroupId == query.GroupId 
                            && m.ChannelId == query.ChannelId)
                .ToListAsync();
            return groupMessageDtos;
        }
    }
}