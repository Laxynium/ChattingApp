using System;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class InvitationId :EntityId
    {
        private InvitationId() : base(default) { }
        private InvitationId(Guid value) : base(value)
        {
        }
        public static InvitationId Create() => new(Guid.NewGuid());
        public static InvitationId From(Guid id) => new(id);

    }
}