using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.Queries.ViewModels;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Application.Queries
{
    public class GetMessageQuery : IQuery<GroupMessageView>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid MessageId { get; }

        public GetMessageQuery(Guid userId, Guid groupId, Guid channelId, Guid messageId)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
            MessageId = messageId;
        }
    }
    public class GetMessageQueryHandler : IQueryHandler<GetMessageQuery, GroupMessageView>
    {
        private readonly GroupsContext _groupsContext;

        public GetMessageQueryHandler(GroupsContext groupsContext)
        { 
            _groupsContext = groupsContext;
        }

        public async Task<GroupMessageView> HandleAsync(GetMessageQuery query)
        {
            var groupMessageDtos = await _groupsContext.GroupMessages
                .Where(m => m.GroupId == query.GroupId
                            && m.ChannelId == query.ChannelId)
                .Where(m=>m.MessageId == query.MessageId)
                .FirstOrDefaultAsync();
            return groupMessageDtos;
        }
    }

}