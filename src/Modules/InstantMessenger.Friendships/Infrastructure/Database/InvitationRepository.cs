using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Friendships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Friendships.Infrastructure.Database
{
    internal sealed class InvitationRepository : IInvitationRepository
    {
        private readonly FriendshipsContext _context;

        public InvitationRepository(FriendshipsContext context) 
            => _context = context;

        public async Task AddAsync(Invitation invitation) 
            => await _context.AddAsync(invitation);

        public async Task<Invitation> GetAsync(InvitationId invitationId) 
            => await _context.Invitations.FirstOrDefaultAsync(x=>x.Id == invitationId);
    }
}