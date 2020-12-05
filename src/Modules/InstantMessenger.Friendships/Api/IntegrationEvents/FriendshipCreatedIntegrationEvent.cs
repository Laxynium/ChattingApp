using System;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.Friendships.Api.IntegrationEvents
{
    public class FriendshipCreatedIntegrationEvent : IIntegrationEvent
    {
        public Guid FriendshipId { get; }
        public Guid FirstPerson { get; }
        public Guid SecondPerson { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipCreatedIntegrationEvent(Guid friendshipId, Guid firstPerson, Guid secondPerson, DateTimeOffset createdAt)
        {
            FriendshipId = friendshipId;
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
            CreatedAt = createdAt;
        }
    }
}