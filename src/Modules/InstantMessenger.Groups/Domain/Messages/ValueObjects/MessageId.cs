using System;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Groups.Domain.Messages.ValueObjects
{
    public class MessageId : EntityId
    {
        private MessageId() : base(default) { }
        private MessageId(Guid value) : base(value)
        {
        }
        public static MessageId Create() => new MessageId(Guid.NewGuid());
        public static MessageId From(Guid id) => new MessageId(id);

    }
}