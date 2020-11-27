using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class InvitationId :SimpleValueObject<Guid>
    {
        private InvitationId() : base(default) { }
        private InvitationId(Guid value) : base(value)
        {
        }
        public static InvitationId Create() => new InvitationId(Guid.NewGuid());
        public static InvitationId From(Guid id) => new InvitationId(id);

    }
}