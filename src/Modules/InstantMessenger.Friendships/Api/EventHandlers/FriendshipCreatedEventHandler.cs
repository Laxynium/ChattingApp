using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Hubs;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Shared.Events;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Api.EventHandlers
{
    public class FriendshipCreatedEventHandler : IEventHandler<FriendshipCreatedEvent>
    {
        private readonly IHubContext<FriendshipsHub, IFriendshipsInterface> _hubContext;

        public FriendshipCreatedEventHandler(IHubContext<FriendshipsHub,IFriendshipsInterface> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(FriendshipCreatedEvent @event)
        {
            //TODO: Updated it to correct logic
            await _hubContext.Clients.All.OnFriendshipCreated(new FriendshipDto(
                @event.FriendshipId,
                @event.FirstPerson,
                @event.SecondPerson,
                @event.CreatedAt
            ));
            //await _hubContext.Clients.All.OnFriendshipCreated(
            //    new FriendshipDto(@event.FriendshipId, @event.FirstPerson, @event.SecondPerson, @event.CreatedAt)
            //);
            //await _hubContext.Clients.Users(@event.FirstPerson.ToString(), @event.SecondPerson.ToString())
            //    .OnFriendshipCreated(new FriendshipDto(@event.FriendshipId, @event.FirstPerson, @event.SecondPerson, @event.CreatedAt));
        }
    }
}