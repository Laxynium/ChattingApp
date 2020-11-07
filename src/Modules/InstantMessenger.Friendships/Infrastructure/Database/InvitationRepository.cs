using System;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;

namespace InstantMessenger.Friendships.Infrastructure.Database
{
    internal sealed class InvitationRepository : IInvitationRepository
    {
        private readonly FriendshipsContext _context;

        public InvitationRepository(FriendshipsContext context) 
            => _context = context;

        public async Task AddAsync(Invitation invitation) 
            => await _context.AddAsync(invitation);

        public async Task<Invitation> GetAsync(Guid invitationId) 
            => await _context.Invitations.FindAsync(invitationId);
    }
}