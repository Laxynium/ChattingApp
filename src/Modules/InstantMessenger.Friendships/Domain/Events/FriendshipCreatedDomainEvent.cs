using System;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Friendships.Domain.Events
{
    public class FriendshipCreatedDomainEvent : IDomainEvent
    {
        public FriendshipId FriendshipId { get; }
        public PersonId FirstPerson { get; }
        public PersonId SecondPerson { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipCreatedDomainEvent(FriendshipId friendshipId, PersonId firstPerson, PersonId secondPerson, DateTimeOffset createdAt)
        {
            FriendshipId = friendshipId;
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
            CreatedAt = createdAt;
        }
    }
}