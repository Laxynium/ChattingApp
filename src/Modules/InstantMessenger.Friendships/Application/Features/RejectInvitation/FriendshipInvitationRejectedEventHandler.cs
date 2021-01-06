using System.Threading.Tasks;
using InstantMessenger.Friendships.Application.Hubs;
using InstantMessenger.Friendships.Application.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Application.Features.RejectInvitation
{
    public class FriendshipInvitationRejectedEventHandler : IIntegrationEventHandler<FriendshipInvitationRejectedIntegrationEvent>
    {
        private readonly IHubContext<FriendshipsHub, IFriendshipsContract> _hubContext;

        public FriendshipInvitationRejectedEventHandler(IHubContext<FriendshipsHub, IFriendshipsContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(FriendshipInvitationRejectedIntegrationEvent @event)
        {
            await _hubContext.Clients.Users(@event.SenderId.ToString("N"), @event.ReceiverId.ToString("N")).OnFriendshipInvitationRejected(
                new InvitationDto(@event.InvitationId, @event.SenderId, @event.ReceiverId, @event.CreatedAt)
            );
        }
    }
}