using System.Threading.Tasks;

namespace InstantMessenger.Friendships.Domain
{
    public interface IFriendshipRepository
    {
        Task AddAsync(Friendship friendship);
    }
}