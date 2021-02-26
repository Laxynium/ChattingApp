using System;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.PrivateMessages.Domain.ValueObjects
{
   public class MessageId : EntityId
    {
        private MessageId():base(default){}
        private MessageId(Guid value) : base(value)
        {
        }
        public new static MessageId Create()=>new(Guid.NewGuid());
        public static MessageId From(Guid id) => new(id);
    }
}