using System;
using CSharpFunctionalExtensions;
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
    }
}