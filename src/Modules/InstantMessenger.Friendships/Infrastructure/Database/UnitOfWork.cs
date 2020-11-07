using System.Threading.Tasks;
using InstantMessenger.Friendships.Api;

namespace InstantMessenger.Friendships.Infrastructure.Database
{
    internal sealed class UnitOfWork: IUnitOfWork
    {
        private readonly FriendshipsContext _context;

        public UnitOfWork(FriendshipsContext context)
        {
            _context = context;
        }
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}