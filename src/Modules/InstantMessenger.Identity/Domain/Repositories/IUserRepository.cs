using System;
using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.ValueObjects;

namespace InstantMessenger.Identity.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetAsync(Guid userId);
        Task<User> GetAsync(string email);
    }
}