using System;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Friendships.Domain.Entities
{
    public class PersonId: EntityId
    {
        public PersonId(Guid value) : base(value)
        {
        }
    }
}