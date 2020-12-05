using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IDomainEventsAccessor _accessor;
        private readonly IEventDispatcher _eventDispatcher;

        public UnitOfWork(DbContext context, IDomainEventsAccessor accessor, IEventDispatcher eventDispatcher)
        {
            _context = context;
            _accessor = accessor;
            _eventDispatcher = eventDispatcher;
        }
        public async Task Commit()
        {
            await PublishDomainEvents();

            await _context.SaveChangesAsync();
        }

        private async Task PublishDomainEvents()
        {
            var domainEvents = _accessor.Events.ToList();
            _accessor.ClearAllDomainEvents();
            var tasks = domainEvents.Select(e => _eventDispatcher.PublishAsync(e));
            await Task.WhenAll(tasks);
        }
    }
}