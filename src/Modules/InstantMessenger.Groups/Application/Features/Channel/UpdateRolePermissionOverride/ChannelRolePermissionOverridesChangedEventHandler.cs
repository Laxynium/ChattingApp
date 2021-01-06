using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.Hubs;
using InstantMessenger.Groups.Application.IntegrationEvents;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Application.Features.Channel.UpdateRolePermissionOverride
{
    public class ChannelRolePermissionOverridesChangedEventHandler : IIntegrationEventHandler<ChannelRolePermissionOverridesChangedEvent>
    {
        private readonly IHubContext<GroupsHub, IGroupsHubContract> _contract;
        private readonly GroupsContext _groupsContext;

        public ChannelRolePermissionOverridesChangedEventHandler(IHubContext<GroupsHub, IGroupsHubContract> contract, GroupsContext groupsContext)
        {
            _contract = contract;
            _groupsContext = groupsContext;
        }
        public async Task HandleAsync(ChannelRolePermissionOverridesChangedEvent @event)
        {
            var members = (await _groupsContext.Groups
                    .AsNoTracking()
                    .Where(g => g.Id == GroupId.From(@event.GroupId))
                    .SelectMany(g => g.Members)
                    .Select(m => m.UserId)
                    .ToListAsync())
                .Select(id => id.Value.ToString("N"))
                .ToList();

            await _contract.Clients.Users(members).OnRolePermissionOverridesChanged(@event);
            await _contract.Clients.Users(members).OnAllowedActionsChanged(new AllowedActionDto(@event.GroupId));
        }
    }
}