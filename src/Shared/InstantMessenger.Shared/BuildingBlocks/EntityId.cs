using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Shared.BuildingBlocks
{
    public abstract class EntityId : SimpleValueObject<Guid>
    {
        protected EntityId(Guid value) : base(value)
        {
        }
    }
}