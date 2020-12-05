using System.Collections.Generic;
using System.Linq;
using InstantMessenger.Shared.BuildingBlocks;
using InstantMessenger.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public class DomainEventsAccessor : IDomainEventsAccessor
    {
        private readonly DbContext _context;

        public DomainEventsAccessor(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<IEvent> Events => _context.ChangeTracker.Entries<Aggregate>()
            .Where(x => x.Entity.DomainEvents.Any())
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        public void ClearAllDomainEvents() => _context.ChangeTracker.Entries<Aggregate>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList()
            .ForEach(e => e.Entity.ClearDomainEvents());
    }
}