using System.Threading.Tasks;
using InstantMessenger.Friendships.Application.Hubs;
using InstantMessenger.Friendships.Application.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Application.Features.RemoveFromFriendships
{
    public class FriendshipRemovedEventHandler : IIntegrationEventHandler<FriendshipRemovedIntegrationEvent>
    {
        private readonly IHubContext<FriendshipsHub, IFriendshipsContract> _hubContext;

        public FriendshipRemovedEventHandler(IHubContext<FriendshipsHub, IFriendshipsContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(FriendshipRemovedIntegrationEvent @event)
        {
            await _hubContext.Clients
                .Users(@event.FirstPerson.ToString("N"), @event.SecondPerson.ToString("N"))
                .OnFriendshipRemoved(new FriendshipDto(@event.FriendshipId, @event.FirstPerson, @event.SecondPerson, @event.CreatedAt));
        }
    }
}