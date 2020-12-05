using System;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Friendships.Domain.ValueObjects
{
    public class FriendshipId: EntityId
    {
        public FriendshipId(Guid value) : base(value)
        {
        }
        public new static FriendshipId Create() => new FriendshipId(Guid.NewGuid());
    }
}