using System;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Friendships.Domain.ValueObjects
{
    public class InvitationId: EntityId
    {
        public InvitationId(Guid value) : base(value)
        {
        }
        public new static InvitationId Create()=>new(Guid.NewGuid());
    }
}