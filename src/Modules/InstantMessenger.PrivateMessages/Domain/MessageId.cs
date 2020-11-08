using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.PrivateMessages.Domain
{
   public class MessageId : SimpleValueObject<Guid>
    {
        private MessageId():base(default){}
        private MessageId(Guid value) : base(value)
        {
        }
        public static MessageId Create()=>new MessageId(Guid.NewGuid());
        public static MessageId From(Guid id) => new MessageId(id);
    }
}