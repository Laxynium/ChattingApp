using System;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class GroupId : EntityId
    {
        private GroupId():base(default){}
        private GroupId(Guid value) : base(value)
        {
        }
        public static GroupId Create() => new GroupId(Guid.NewGuid());
        public static GroupId From(Guid id) => new GroupId(id);
        
    }
}