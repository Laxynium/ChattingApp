using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Hubs;
using InstantMessenger.Friendships.Api.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Api.Features.AcceptInvitation
{
    public class FriendshipInvitationAcceptedEventHandler : IIntegrationEventHandler<FriendshipInvitationAcceptedIntegrationEvent>
    {
        private readonly IHubContext<FriendshipsHub, IFriendshipsContract> _hubContext;

        public FriendshipInvitationAcceptedEventHandler(IHubContext<FriendshipsHub, IFriendshipsContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(FriendshipInvitationAcceptedIntegrationEvent @event)
        {
            await _hubContext.Clients.Users(@event.SenderId.ToString("N"), @event.ReceiverId.ToString("N")).OnFriendshipInvitationAccepted(
                new InvitationDto(@event.InvitationId, @event.SenderId, @event.ReceiverId, @event.CreatedAt)
            );
        }
    }
}