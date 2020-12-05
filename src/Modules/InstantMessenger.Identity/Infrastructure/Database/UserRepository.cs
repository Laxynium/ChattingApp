using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Identity.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Identity.Infrastructure.Database
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityContext _context;

        public UserRepository(IdentityContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User> GetAsync(UserId userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x=>x.Id == userId) ?? _context.Users.Local.FirstOrDefault(x=>x.Id == userId);
        }

        public async Task<User> GetAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email.Value == email);
        }
    }
}