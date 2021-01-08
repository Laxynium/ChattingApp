using System;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Friendships.Domain.ValueObjects
{
    public class PersonId: EntityId
    {
        public PersonId(Guid value) : base(value)
        {
        }
    }
}