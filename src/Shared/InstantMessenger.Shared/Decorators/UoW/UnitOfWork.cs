using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public class UnitOfWork<TDbContext> where TDbContext:DbContext
    {
        private readonly TDbContext _context;
        private readonly DomainEventPublisher<TDbContext> _publisher;

        public UnitOfWork(TDbContext context, DomainEventPublisher<TDbContext> publisher)
        {
            _context = context;
            _publisher = publisher;
        }
        public async Task Commit()
        {
            await _publisher.Publish();

            await _context.SaveChangesAsync();
        }
    }
}