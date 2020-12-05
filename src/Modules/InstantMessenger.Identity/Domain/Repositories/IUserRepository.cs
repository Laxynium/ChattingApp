using System;
using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Identity.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetAsync(UserId userId);
        Task<User> GetAsync(string email);
    }
}