using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Repositories
{
    public interface IGroupRepository
    {
        Task AddAsync(Group group);
        Task<Group> GetAsync(GroupId id);
        Task<bool> ExistsAsync(GroupId id);
        Task RemoveAsync(Group group);
    }
}