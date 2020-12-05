using System;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.PrivateMessages.Domain.ValueObjects
{
    public class ConversationId : EntityId
    {
        private ConversationId():base(default){}
        public ConversationId(Guid value) : base(value)
        {
        }
        public new static ConversationId Create() => new ConversationId(Guid.NewGuid());
    }
}