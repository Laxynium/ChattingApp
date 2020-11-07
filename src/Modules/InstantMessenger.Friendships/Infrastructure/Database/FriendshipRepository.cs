using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;

namespace InstantMessenger.Friendships.Infrastructure.Database
{
    internal sealed class FriendshipRepository : IFriendshipRepository
    {
        private readonly FriendshipsContext _context;

        public FriendshipRepository(FriendshipsContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Friendship friendship)
        {
            await _context.Friendships.AddAsync(friendship);
        }
    }
}