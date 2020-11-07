using System;
using System.Threading.Tasks;

namespace InstantMessenger.Friendships.Domain
{
    public interface IPersonRepository
    {
        Task<bool> ExistsAsync(Guid personId);
        Task AddAsync(Person person);
        Task<Person> GetAsync(Guid personId);
    }
}