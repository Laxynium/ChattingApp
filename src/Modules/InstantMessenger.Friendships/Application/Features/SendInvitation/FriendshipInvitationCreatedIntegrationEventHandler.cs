using System.Threading.Tasks;
using InstantMessenger.Friendships.Application.Hubs;
using InstantMessenger.Friendships.Application.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Application.Features.SendInvitation
{
    public class FriendshipInvitationCreatedIntegrationEventHandler : IIntegrationEventHandler<FriendshipInvitationCreatedIntegrationEvent>
    {
        private readonly IHubContext<FriendshipsHub, IFriendshipsContract> _hubContext;

        public FriendshipInvitationCreatedIntegrationEventHandler(IHubContext<FriendshipsHub, IFriendshipsContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(FriendshipInvitationCreatedIntegrationEvent @event)
        {
            await _hubContext.Clients.Users(@event.SenderId.ToString("N"), @event.ReceiverId.ToString("N")).OnFriendshipInvitationCreated(
                new InvitationDto(@event.InvitationId, @event.SenderId, @event.ReceiverId, @event.CreatedAt)
            );
        }
    }
}