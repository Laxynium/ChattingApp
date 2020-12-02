using System;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Friendships.Domain
{
    [ProccessAlsoInternally]
    public class FriendshipCreatedEvent : IEvent
    {
        public Guid FriendshipId { get; }
        public Guid FirstPerson { get; }
        public Guid SecondPerson { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipCreatedEvent(Guid friendshipId, Guid firstPerson, Guid secondPerson, DateTimeOffset createdAt)
        {
            FriendshipId = friendshipId;
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
            CreatedAt = createdAt;
        }
    }
}