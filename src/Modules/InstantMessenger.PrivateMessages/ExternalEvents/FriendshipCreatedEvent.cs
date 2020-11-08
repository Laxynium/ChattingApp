using System;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.PrivateMessages.ExternalEvents
{
    public class FriendshipCreatedEvent : IEvent
    {
        public Guid FriendshipId { get; }
        public Guid FirstPerson { get; }
        public Guid SecondPerson { get; }

        public FriendshipCreatedEvent(Guid friendshipId, Guid firstPerson, Guid secondPerson)
        {
            FriendshipId = friendshipId;
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
        }
    }
}