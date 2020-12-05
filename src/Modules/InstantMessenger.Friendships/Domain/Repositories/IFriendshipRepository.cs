using System;
using System.Threading.Tasks;

namespace InstantMessenger.Friendships.Domain.Repositories
{
    public interface IFriendshipRepository
    {
        Task AddAsync(Friendship friendship);
        Task<bool> ExistsBetweenAsync(Guid senderId, Guid receiverId);
        Task<Friendship> GetAsync(Guid friendshipId);
        Task RemoveAsync(Friendship friendship);
    }
}