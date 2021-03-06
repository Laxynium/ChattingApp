using System.Collections.Generic;
using System.Linq;
using InstantMessenger.Shared.BuildingBlocks;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.Modules;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Shared.UoW.EFCore
{
    internal sealed class EFCoreDomainEventAccessor<TModule> : IDomainEventAccessor<TModule> where TModule : IModule
    {
        private readonly DbContext _context;

        public EFCoreDomainEventAccessor(DbContext context)
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