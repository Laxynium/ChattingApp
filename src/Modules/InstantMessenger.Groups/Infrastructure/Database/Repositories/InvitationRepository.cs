using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Infrastructure.Database.Repositories
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

        public async Task<Invitation> GetAsync(InvitationId id)
        {
            return await _context.Invitations.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task RemoveAsync(Invitation invitation)
        {
            _context.Remove(invitation);
            return Task.CompletedTask;
        }
    }
}