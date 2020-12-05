using System.Collections.Generic;
using System.Linq;
using InstantMessenger.Shared.BuildingBlocks;
using InstantMessenger.Shared.Messages.Events;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public class DomainEventsAccessor<TDbContext> where TDbContext:DbContext
    {
        private readonly TDbContext _context;

        public DomainEventsAccessor(TDbContext context)
        {
            _context = context;
        }

        public IEnumerable<IDomainEvent> Events => _context.ChangeTracker.Entries<IEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        public void ClearAllDomainEvents() => _context.ChangeTracker.Entries<IEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList()
            .ForEach(e => e.Entity.ClearDomainEvents());
    }
}