using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.Rules;
using InstantMessenger.Friendships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Friendships.Infrastructure.Database.Rules
{
    internal sealed class UniquePendingInvitationRule : IUniquePendingInvitationRule
    {
        private readonly FriendshipsContext _context;

        public UniquePendingInvitationRule(FriendshipsContext context)
        {
            _context = context;
        }
        public async Task<bool> IsMet(PersonId sender, PersonId receiver) => !await _context.Invitations.AnyAsync(
            x => 
                x.Status == InvitationStatus.Pending &&
                (x.SenderId == sender && x.ReceiverId == receiver || 
                x.SenderId == receiver && x.ReceiverId == sender)
        );
    }
}