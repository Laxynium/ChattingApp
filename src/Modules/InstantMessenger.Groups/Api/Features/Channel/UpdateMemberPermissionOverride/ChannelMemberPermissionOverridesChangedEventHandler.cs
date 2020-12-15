using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Hubs;
using InstantMessenger.Groups.Api.IntegrationEvents;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Features.Channel.UpdateMemberPermissionOverride
{
    public class ChannelMemberPermissionOverridesChangedEventHandler : IIntegrationEventHandler<ChannelMemberPermissionOverridesChangedEvent>
    {
        private readonly IHubContext<GroupsHub, IGroupsHubContract> _contract;
        private readonly GroupsContext _groupsContext;

        public ChannelMemberPermissionOverridesChangedEventHandler(IHubContext<GroupsHub, IGroupsHubContract> contract, GroupsContext groupsContext)
        {
            _contract = contract;
            _groupsContext = groupsContext;
        }
        public async Task HandleAsync(ChannelMemberPermissionOverridesChangedEvent @event)
        {
            var members = (await _groupsContext.Groups
                    .AsNoTracking()
                    .Where(g => g.Id == GroupId.From(@event.GroupId))
                    .SelectMany(g => g.Members)
                    .Select(m => m.UserId)
                    .ToListAsync())
                .Select(id => id.Value.ToString("N"))
                .ToList();

            await _contract.Clients.Users(members).OnMemberPermissionOverridesChanged(@event);
            await _contract.Clients.Users(members).OnAllowedActionsChanged(new AllowedActionDto(@event.GroupId));
        }
    }
}