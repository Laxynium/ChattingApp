using System.Threading.Tasks;

namespace InstantMessenger.Groups.Domain
{
    public interface IGroupRepository
    {
        Task AddAsync(Domain.Group group);
    }
}