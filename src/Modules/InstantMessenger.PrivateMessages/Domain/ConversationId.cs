using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.PrivateMessages.Domain
{
    public class ConversationId : SimpleValueObject<Guid>
    {
        private ConversationId():base(default){}
        public ConversationId(Guid value) : base(value)
        {
        }
        public static ConversationId Create() => new ConversationId(Guid.NewGuid());
    }
}