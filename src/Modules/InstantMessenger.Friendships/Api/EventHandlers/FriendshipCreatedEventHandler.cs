﻿using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Hubs;
using InstantMessenger.Friendships.Domain.Events;
using InstantMessenger.Shared.Events;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Api.EventHandlers
{
    public class FriendshipCreatedEventHandler : IEventHandler<FriendshipCreatedEvent>
    {
        private readonly IHubContext<FriendshipsHub, IFriendshipsContract> _hubContext;

        public FriendshipCreatedEventHandler(IHubContext<FriendshipsHub,IFriendshipsContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(FriendshipCreatedEvent @event)
        {
            await _hubContext.Clients
                .Users(@event.FirstPerson.ToString("N"), @event.SecondPerson.ToString("N"))
                .OnFriendshipCreated(new FriendshipDto(@event.FriendshipId, @event.FirstPerson, @event.SecondPerson, @event.CreatedAt));
        }
    }
}