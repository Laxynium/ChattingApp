using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Hubs;
using InstantMessenger.Friendships.Domain.Events;
using InstantMessenger.Shared.Events;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Api.EventHandlers
{
    public class FriendshipInvitationCreatedEventHandler : IEventHandler<FriendshipInvitationCreatedEvent>
    {
        private readonly IHubContext<FriendshipsHub, IFriendshipsInterface> _hubContext;

        public FriendshipInvitationCreatedEventHandler(IHubContext<FriendshipsHub, IFriendshipsInterface> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(FriendshipInvitationCreatedEvent @event)
        {
            await _hubContext.Clients.Users(@event.SenderId.ToString("N"), @event.ReceiverId.ToString("N")).OnFriendshipInvitationCreated(
                new InvitationDto(@event.InvitationId, @event.SenderId, @event.ReceiverId, @event.CreatedAt)
            );
        }
    }
}