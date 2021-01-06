using System;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly GroupsViewContext _viewContext;
        private readonly GroupsContext _groupsContext;

        public GetMessageQueryHandler(GroupsViewContext viewContext, GroupsContext groupsContext)
        {
            _viewContext = viewContext;
            _groupsContext = groupsContext;
        }

        public async Task<GroupMessageView> HandleAsync(GetMessageQuery query)
        {
            var groupMessageDtos = await _viewContext.GroupMessages
                .Where(m => m.GroupId == query.GroupId
                            && m.ChannelId == query.ChannelId)
                .Where(m=>m.MessageId == query.MessageId)
                .FirstOrDefaultAsync();
            return groupMessageDtos;
        }
    }

}