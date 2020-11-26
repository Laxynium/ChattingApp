using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class ChannelId : SimpleValueObject<Guid>
    {
        private ChannelId() : base(default) { }
        private ChannelId(Guid value) : base(value)
        {
        }
        public static ChannelId Create() => new ChannelId(Guid.NewGuid());
        public static ChannelId From(Guid id) => new ChannelId(id);
    }
}