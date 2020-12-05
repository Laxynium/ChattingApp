using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Hubs;
using InstantMessenger.Friendships.Api.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Api.Features.SendInvitation
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