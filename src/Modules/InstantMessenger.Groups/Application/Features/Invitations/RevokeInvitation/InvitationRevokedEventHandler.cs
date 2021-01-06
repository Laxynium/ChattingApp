using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.Hubs;
using InstantMessenger.Groups.Application.IntegrationEvents;
using InstantMessenger.Groups.Application.ResponseDtos;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Application.Features.Invitations.RevokeInvitation
{
    public class InvitationRevokedEventHandler : IIntegrationEventHandler<InvitationRevokedEvent>
    {
        private readonly IHubContext<GroupsHub, IGroupsHubContract> _hubContext;
        private readonly GroupsContext _groupsContext;

        public InvitationRevokedEventHandler(IHubContext<GroupsHub,IGroupsHubContract> hubContext, GroupsContext groupsContext)
        {
            _hubContext = hubContext;
            _groupsContext = groupsContext;
        }
        public async Task HandleAsync(InvitationRevokedEvent @event)
        {
            var group = await _groupsContext.Groups.AsNoTracking()
                .Where(g => g.Id == GroupId.From(@event.GroupId))
                .FirstOrDefaultAsync();

            var users = group.Members.Where(m => group.CanAccessInvitations(m.UserId))
                .Select(u => u.UserId.Value.ToString("N"))
                .ToList();

            await _hubContext.Clients.Users(users).OnInvitationRevoked(new InvitationDto(@event.GroupId, @event.InvitationId, @event.Code, 
                @event.ExpirationTime, @event.UsageCounter));
        }
    }
}