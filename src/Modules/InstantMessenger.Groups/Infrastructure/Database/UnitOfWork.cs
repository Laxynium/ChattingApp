using System.Threading.Tasks;
using InstantMessenger.Groups.Api;

namespace InstantMessenger.Groups.Infrastructure.Database
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly GroupsContext _context;

        public UnitOfWork(GroupsContext context)
        {
            _context = context;
        }
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}