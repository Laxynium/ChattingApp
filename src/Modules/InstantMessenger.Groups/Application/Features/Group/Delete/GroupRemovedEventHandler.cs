using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.Hubs;
using InstantMessenger.Groups.Application.ResponseDtos;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Groups.Application.Features.Group.Delete
{
    public class GroupRemovedEventHandler: IIntegrationEventHandler<IntegrationEvents.GroupRemovedEvent>
    {
        private readonly IHubContext<GroupsHub, IGroupsHubContract> _hubContext;

        public GroupRemovedEventHandler(IHubContext<GroupsHub, IGroupsHubContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(IntegrationEvents.GroupRemovedEvent @event)
        {
            await _hubContext.Clients.Users(@event.AllowedUsers.Select(id => id.ToString("N")).ToList())
                .OnGroupRemoved(new GroupDto(@event.GroupId, @event.GroupName,@event.CreatedAt, @event.OwnerId));
        }
    }
}