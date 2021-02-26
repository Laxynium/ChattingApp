using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class MemberId : SimpleValueObject<Guid>
    {
        private MemberId() : base(default) { }
        private MemberId(Guid value) : base(value)
        {
        }
        public static MemberId Create() => new(Guid.NewGuid());
        public static MemberId From(Guid id) => new(id);

    }
}