using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.Shared.BuildingBlocks
{
    public abstract class Aggregate: Entity<EntityId>
    {
        private readonly List<IEvent> _events = new List<IEvent>();
        public IEnumerable<IEvent> DomainEvents => _events.ToList();
        protected void Apply(IEvent @event) => _events.Add(@event);
        public void ClearDomainEvents() => _events.Clear();
    }
}