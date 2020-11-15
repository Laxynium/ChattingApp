using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class RoleId : SimpleValueObject<Guid>
    {
        private RoleId() : base(default) { }
        private RoleId(Guid value) : base(value)
        {
        }
        public static RoleId Create() => new RoleId(Guid.NewGuid());
        public static RoleId From(Guid id) => new RoleId(id);

    }
}