using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Shared.BuildingBlocks
{
    public class EntityId : SimpleValueObject<Guid>
    {
        public EntityId(Guid value) : base(value)
        {
        }
    }
}