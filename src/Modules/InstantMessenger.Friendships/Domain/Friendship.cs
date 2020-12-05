using System;
using CSharpFunctionalExtensions;
using InstantMessenger.Friendships.Domain.Exceptions;
using NodaTime;

namespace InstantMessenger.Friendships.Domain
{
    public class Friendship : Entity<Guid>
    {
        public Guid FirstPerson { get; }
        public Guid SecondPerson { get; }
        public DateTimeOffset CreatedAt { get; }

        private Friendship(){}
        private Friendship(Guid id, Guid firstPerson, Guid secondPerson, DateTimeOffset createdAt):base(id)
        {
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
            CreatedAt = createdAt;
        }

        public static Friendship Create(Guid firstPerson, Guid secondPerson, IClock clock)
        {
            return new Friendship(Guid.NewGuid(), firstPerson, secondPerson,clock.GetCurrentInstant().InUtc().ToDateTimeOffset());
        }

        public void Remove(Person person)
        {
            if(FirstPerson != person.Id && SecondPerson != person.Id)
                throw new FriendshipNotFoundException(Id);
        }
    }
}