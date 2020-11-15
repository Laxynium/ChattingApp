using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class UserId : SimpleValueObject<Guid>
    {
        private UserId() : base(default) { }
        private UserId(Guid value) : base(value)
        {
        }
        public static UserId Create() => new UserId(Guid.NewGuid());
        public static UserId From(Guid id) => new UserId(id);

    }
}