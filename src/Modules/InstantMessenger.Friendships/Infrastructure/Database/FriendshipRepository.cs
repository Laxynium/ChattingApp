using System;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Friendships.Domain.ValueObjects;
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

        public Task<bool> ExistsBetweenAsync(PersonId senderId, PersonId receiverId)
        {
            return _context.Friendships.AnyAsync(
                x => 
                     x.FirstPerson == senderId && x.SecondPerson == receiverId || 
                     x.FirstPerson == receiverId && x.SecondPerson == senderId
            );
        }

        public async Task<Friendship> GetAsync(FriendshipId friendshipId)
        {
            return await _context.Friendships.FirstOrDefaultAsync(x => x.Id == friendshipId);
        }

        public Task RemoveAsync(Friendship friendship)
        {
            _context.Remove(friendship);
            return Task.CompletedTask;
        }
    }
}