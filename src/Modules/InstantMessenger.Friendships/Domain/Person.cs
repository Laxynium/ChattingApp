using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Friendships.Domain
{
    public class Person : Entity<Guid>
    {
        public Person(Guid id) : base(id)
        {
            
        }
    }
}