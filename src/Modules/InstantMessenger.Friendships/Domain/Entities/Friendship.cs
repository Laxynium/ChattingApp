using System;
using InstantMessenger.Friendships.Domain.Events;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;
using NodaTime;

namespace InstantMessenger.Friendships.Domain.Entities
{
    public class Friendship : Entity<FriendshipId>
    {
        public PersonId FirstPerson { get; }
        public PersonId SecondPerson { get; }
        public DateTimeOffset CreatedAt { get; }

        private Friendship(){}
        private Friendship(FriendshipId id, PersonId firstPerson, PersonId secondPerson, DateTimeOffset createdAt):base(id)
        {
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
            CreatedAt = createdAt;
            Apply(new FriendshipCreatedDomainEvent(id,firstPerson, secondPerson,createdAt));
        }

        public static Friendship Create(PersonId firstPerson, PersonId secondPerson, IClock clock)
        {
            return new Friendship(FriendshipId.Create(), firstPerson, secondPerson,clock.GetCurrentInstant().InUtc().ToDateTimeOffset());
        }

        public void Remove(PersonId person)
        {
            if(FirstPerson != person && SecondPerson != person)
                throw new FriendshipNotFoundException(Id);

            Apply(new FriendshipRemovedDomainEvent(Id, FirstPerson, SecondPerson, CreatedAt));
        }
    }
}