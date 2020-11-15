using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Rules;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Identity.Infrastructure.Database
{
    internal sealed class UniqueNicknameRule : IUniqueNicknameRule
    {
        private readonly IdentityContext _context;

        public UniqueNicknameRule(IdentityContext context)
        {
            _context = context;
        }
        public async Task<bool> IsMeet(Nickname nickname) => await _context.Users.AllAsync(x => x.Nickname != nickname);
    }
}