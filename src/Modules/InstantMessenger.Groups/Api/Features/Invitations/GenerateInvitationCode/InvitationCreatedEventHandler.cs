using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Hubs;
using InstantMessenger.Groups.Api.IntegrationEvents;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Features.Invitations.GenerateInvitationCode
{
    public class InvitationRevokedEventHandler : IIntegrationEventHandler<InvitationCreatedEvent>
    {
        private readonly IHubContext<GroupsHub, IGroupsHubContract> _hubContext;
        private readonly GroupsContext _groupsContext;

        public InvitationRevokedEventHandler(IHubContext<GroupsHub,IGroupsHubContract> hubContext, GroupsContext groupsContext)
        {
            _hubContext = hubContext;
            _groupsContext = groupsContext;
        }
        public async Task HandleAsync(InvitationCreatedEvent @event)
        {
            var group = await _groupsContext.Groups.AsNoTracking()
                .Where(g => g.Id == GroupId.From(@event.GroupId))
                .FirstOrDefaultAsync();

            var users = group.Members.Where(m => group.CanAccessInvitations(m.UserId))
                .Select(u => u.UserId.Value.ToString("N"))
                .ToList();

            await _hubContext.Clients.Users(users).OnInvitationCreated(new InvitationDto(@event.GroupId, @event.InvitationId, @event.Code));
        }
    }
}