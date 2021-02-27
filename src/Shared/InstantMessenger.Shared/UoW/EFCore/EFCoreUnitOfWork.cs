using System.Threading.Tasks;
using InstantMessenger.Shared.Modules;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Shared.UoW.EFCore
{
    internal sealed class EFCoreUnitOfWork<TModule> : IUnitOfWork<TModule> where TModule : IModule
    {
        private readonly DbContext _context;
        private readonly IDomainEventPublisher<TModule> _domainEventPublisher;

        public EFCoreUnitOfWork(DbContext context, IDomainEventPublisher<TModule> domainEventPublisher)
        {
            _context = context;
            _domainEventPublisher = domainEventPublisher;
        }

        public async Task Commit()
        {
            await _domainEventPublisher.Publish();
            
            await _context.SaveChangesAsync();
        }
    }
}