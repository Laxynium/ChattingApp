using System;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class ChannelId : EntityId
    {
        private ChannelId() : base(default) { }
        private ChannelId(Guid value) : base(value)
        {
        }
        public static ChannelId Create() => new(Guid.NewGuid());
        public static ChannelId From(Guid id) => new(id);
    }
}