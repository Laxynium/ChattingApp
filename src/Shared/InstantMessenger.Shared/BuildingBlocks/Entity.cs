using System.Collections.Generic;
using System.Linq;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Shared.BuildingBlocks
{
    public abstract class Entity<TId>: CSharpFunctionalExtensions.Entity<TId>, IEntity where TId: EntityId
    {
        private readonly List<IDomainEvent> _events = new();
        public IEnumerable<IDomainEvent> DomainEvents => _events.ToList();
        protected void Apply(IDomainEvent @event) => _events.Add(@event);
        public void ClearDomainEvents() => _events.Clear();
        protected Entity(TId id = null):base(id){}
    }

    public abstract class Entity : Entity<EntityId>
    {
        protected Entity(EntityId id = null):base(id)
        {
            
        }
    }
}