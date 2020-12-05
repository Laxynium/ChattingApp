using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Hubs;
using InstantMessenger.Friendships.Api.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Api.Features.CancelInvitation
{
    public class FriendshipInvitationCanceledEventHandler : IIntegrationEventHandler<FriendshipInvitationCanceledIntegrationEvent>
    {
        private readonly IHubContext<FriendshipsHub, IFriendshipsContract> _hubContext;

        public FriendshipInvitationCanceledEventHandler(IHubContext<FriendshipsHub, IFriendshipsContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(FriendshipInvitationCanceledIntegrationEvent @event)
        {
            await _hubContext.Clients.Users(@event.SenderId.ToString("N"), @event.ReceiverId.ToString("N")).OnFriendshipInvitationCanceled(
                new InvitationDto(@event.InvitationId, @event.SenderId, @event.ReceiverId, @event.CreatedAt)
            );
        }
    }
}