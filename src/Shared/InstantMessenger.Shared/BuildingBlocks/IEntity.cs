using System.Collections.Generic;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Shared.BuildingBlocks
{
    public interface IEntity
    {
        IEnumerable<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}