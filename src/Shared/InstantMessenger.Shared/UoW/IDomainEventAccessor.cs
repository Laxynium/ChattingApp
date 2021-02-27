using System.Collections.Generic;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Shared.UoW
{
    internal interface IDomainEventAccessor<TModule> where TModule : IModule
    {
        IEnumerable<IDomainEvent> Events { get; }
        void ClearAllDomainEvents();
    }
}