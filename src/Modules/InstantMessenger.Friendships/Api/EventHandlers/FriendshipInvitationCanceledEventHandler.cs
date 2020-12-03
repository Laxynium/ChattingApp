using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Hubs;
using InstantMessenger.Friendships.Domain.Events;
using InstantMessenger.Shared.Events;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Api.EventHandlers
{
    public class FriendshipInvitationCanceledEventHandler : IEventHandler<FriendshipInvitationCanceledEvent>
    {
        private readonly IHubContext<FriendshipsHub, IFriendshipsInterface> _hubContext;

        public FriendshipInvitationCanceledEventHandler(IHubContext<FriendshipsHub, IFriendshipsInterface> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(FriendshipInvitationCanceledEvent @event)
        {
            await _hubContext.Clients.Users(@event.SenderId.ToString("N"), @event.ReceiverId.ToString("N")).OnFriendshipInvitationCanceled(
                new InvitationDto(@event.InvitationId, @event.SenderId, @event.ReceiverId, @event.CreatedAt)
            );
        }
    }
}