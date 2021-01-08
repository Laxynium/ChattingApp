using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Rules;
using InstantMessenger.Groups.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Infrastructure.Database.Rules
{
    internal sealed class UniqueInvitationCodeRule : IUniqueInvitationCodeRule
    {
        private readonly GroupsContext _context;

        public UniqueInvitationCodeRule(GroupsContext context)
        {
            _context = context;
        }
        public async Task<bool> IsMeet(InvitationCode invitationCode)
        {
            return !await _context.Invitations.AnyAsync(x => x.InvitationCode == invitationCode);
        }
    }
}