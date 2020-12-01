using System;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Friendships.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public Task<bool> ExistsBetweenAsync(Guid senderId, Guid receiverId)
        {
            return _context.Friendships.AnyAsync(
                x => 
                     x.FirstPerson == senderId && x.SecondPerson == receiverId || 
                     x.FirstPerson == receiverId && x.SecondPerson == senderId
            );
        }
    }
}