using System;
using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Identity.Domain.ValueObjects;
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

        public async Task<User> GetAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email.Value == email);
        }
    }
}