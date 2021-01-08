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
        public PersonId FirstPersonId { get; }
        public PersonId SecondPersonId { get; }
        public DateTimeOffset CreatedAt { get; }

        private Friendship(){}
        private Friendship(FriendshipId id, PersonId firstPersonId, PersonId secondPersonId, DateTimeOffset createdAt):base(id)
        {
            FirstPersonId = firstPersonId;
            SecondPersonId = secondPersonId;
            CreatedAt = createdAt;
            Apply(new FriendshipCreatedDomainEvent(id,firstPersonId, secondPersonId,createdAt));
        }

        public static Friendship Create(PersonId firstPerson, PersonId secondPerson, IClock clock)
        {
            return new Friendship(FriendshipId.Create(), firstPerson, secondPerson,clock.GetCurrentInstant().InUtc().ToDateTimeOffset());
        }

        public void Remove(PersonId personId)
        {
            if(FirstPersonId != personId && SecondPersonId != personId)
                throw new FriendshipNotFoundException(Id);

            Apply(new FriendshipRemovedDomainEvent(Id, FirstPersonId, SecondPersonId, CreatedAt));
        }
    }
}