using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.Hubs;
using InstantMessenger.Groups.Application.IntegrationEvents;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Application.Features.Messages.SendMessage
{
    public class MessageCreatedEventHandler : IIntegrationEventHandler<MessageCreatedEvent>
    {
        private readonly IHubContext<GroupsHub, IGroupsHubContract> _hubContext;
        private readonly GroupsViewContext _viewContext;
        private readonly GroupsContext _context;

        public MessageCreatedEventHandler(IHubContext<GroupsHub, IGroupsHubContract> hubContext, GroupsViewContext viewContext,
            GroupsContext context)
        {
            _hubContext = hubContext;
            _viewContext = viewContext;
            _context = context;
        }
        public async Task HandleAsync(MessageCreatedEvent @event)
        {
            var group = await _context.Groups.AsNoTracking()
                .Where(g => g.Id == GroupId.From(@event.GroupId))
                .FirstOrDefaultAsync();
            var channel = await _context.Channels.AsNoTracking()
                .Where(c => c.GroupId == GroupId.From(@event.GroupId) && c.Id == ChannelId.From(@event.ChannelId))
                .FirstOrDefaultAsync();
            var message = await _viewContext.GroupMessages
                .AsNoTracking()
                .Where(x => x.GroupId == GroupId.From(@event.GroupId))
                .Where(x => x.ChannelId == @event.ChannelId)
                .Where(x => x.MessageId == MessageId.From(@event.MessageId))
                .FirstOrDefaultAsync();
            if (message is null || group is null || channel is null)
                return;
            var allowedMembers = group.Members
                .Where(m => group.CanAccessMessages(m.UserId, channel))
                .Select(m=>m.UserId.Value.ToString("N"))
                .ToList();

            await _hubContext.Clients.Users(allowedMembers).OnMessageCreated(message);
        }
    }
}