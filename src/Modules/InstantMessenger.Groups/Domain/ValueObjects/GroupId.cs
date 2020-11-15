using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class GroupId : SimpleValueObject<Guid>
    {
        private GroupId():base(default){}
        private GroupId(Guid value) : base(value)
        {
        }
        public static GroupId Create() => new GroupId(Guid.NewGuid());
        public static GroupId From(Guid id) => new GroupId(id);
        
    }
}