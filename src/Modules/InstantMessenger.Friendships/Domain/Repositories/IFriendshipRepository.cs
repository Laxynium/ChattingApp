using System;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.ValueObjects;

namespace InstantMessenger.Friendships.Domain.Repositories
{
    public interface IFriendshipRepository
    {
        Task AddAsync(Friendship friendship);
        Task<bool> ExistsBetweenAsync(PersonId senderId, PersonId receiverId);
        Task<Friendship> GetAsync(FriendshipId friendshipId);
        Task RemoveAsync(Friendship friendship);
    }
}