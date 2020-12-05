using System.Collections.Generic;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public interface IDomainEventsAccessor
    {
        IEnumerable<IEvent> Events { get; }
        void ClearAllDomainEvents();
    }
}