using System.Threading.Tasks;
using InstantMessenger.Identity.Api;

namespace InstantMessenger.Identity.Infrastructure.Database
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityContext _context;

        public UnitOfWork(IdentityContext context)
        {
            _context = context;
        }
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}