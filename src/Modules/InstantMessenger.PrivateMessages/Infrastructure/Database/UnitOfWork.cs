using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api;

namespace InstantMessenger.PrivateMessages.Infrastructure.Database
{
    internal sealed class UnitOfWork: IUnitOfWork
    {
        private readonly PrivateMessagesContext _context;

        public UnitOfWork(PrivateMessagesContext context)
        {
            _context = context;
        }
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}