using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public class GetMessagesQuery : IQuery<IEnumerable<GroupMessageDto>>
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
    public class GetMessagesQueryHandler : IQueryHandler<GetMessagesQuery, IEnumerable<GroupMessageDto>>
    {
        private readonly GroupsViewContext _viewContext;

        public GetMessagesQueryHandler(GroupsViewContext viewContext)
        {
            _viewContext = viewContext;
        }

        public async Task<IEnumerable<GroupMessageDto>> HandleAsync(GetMessagesQuery query)
        {
            return await _viewContext.GroupMessages
                .Where(m => m.GroupId == query.GroupId 
                            && m.ChannelId == query.ChannelId)
                .ToListAsync();
        }
    }
}