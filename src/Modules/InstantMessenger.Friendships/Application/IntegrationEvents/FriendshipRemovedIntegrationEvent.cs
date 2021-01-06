using System;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.Friendships.Application.IntegrationEvents
{
    public class FriendshipRemovedIntegrationEvent : IIntegrationEvent
    {
        public Guid FriendshipId { get; }
        public Guid FirstPerson { get; }
        public Guid SecondPerson { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipRemovedIntegrationEvent(Guid friendshipId, Guid firstPerson, Guid secondPerson, DateTimeOffset createdAt)
        {
            FriendshipId = friendshipId;
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
            CreatedAt = createdAt;
        }
    }
}