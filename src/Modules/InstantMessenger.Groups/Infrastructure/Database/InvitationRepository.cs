using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Infrastructure.Database
{
    internal sealed class InvitationRepository : IInvitationRepository
    {
        private readonly GroupsContext _context;

        public InvitationRepository(GroupsContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Invitation invitation)
        {
            await _context.Invitations.AddAsync(invitation);
        }

        public async Task<Invitation> GetAsync(InvitationCode invitationCode) 
            => await _context.Invitations.FirstOrDefaultAsync(x => x.InvitationCode == invitationCode);
    }
}