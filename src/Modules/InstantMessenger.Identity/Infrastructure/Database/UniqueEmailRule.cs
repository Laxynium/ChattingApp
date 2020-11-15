using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Identity.Infrastructure.Database
{
    internal sealed class UniqueEmailRule : IUniqueEmailRule
    {
        private readonly IdentityContext _context;

        public UniqueEmailRule(IdentityContext context)
        {
            _context = context;
        }
        public async Task<bool> IsMet(Email email)
        {
            return !await _context.Users.AnyAsync(x => x.Email.Value == email.Value);
        }
    }
}