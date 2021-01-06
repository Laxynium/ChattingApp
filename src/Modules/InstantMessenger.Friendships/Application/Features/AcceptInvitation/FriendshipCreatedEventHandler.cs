using System.Threading.Tasks;
using InstantMessenger.Friendships.Application.Hubs;
using InstantMessenger.Friendships.Application.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Application.Features.AcceptInvitation
{
    public class FriendshipCreatedEventHandler : IIntegrationEventHandler<FriendshipCreatedIntegrationEvent>
    {
        private readonly IHubContext<FriendshipsHub, IFriendshipsContract> _hubContext;

        public FriendshipCreatedEventHandler(IHubContext<FriendshipsHub,IFriendshipsContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(FriendshipCreatedIntegrationEvent @event)
        {
            await _hubContext.Clients
                .Users(@event.FirstPerson.ToString("N"), @event.SecondPerson.ToString("N"))
                .OnFriendshipCreated(new FriendshipDto(@event.FriendshipId, @event.FirstPerson, @event.SecondPerson, @event.CreatedAt));
        }
    }
}