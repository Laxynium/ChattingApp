using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Friendships.Domain.Rules;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Friendships.Infrastructure.Database
{
    internal sealed class UniquePendingInvitationRule : IUniquePendingInvitationRule
    {
        private readonly FriendshipsContext _context;

        public UniquePendingInvitationRule(FriendshipsContext context)
        {
            _context = context;
        }
        public async Task<bool> IsMet(Person sender, Person receiver) => !await _context.Invitations.AnyAsync(
            x => 
                x.Status == InvitationStatus.Pending &&
                (x.SenderId == sender.Id && x.ReceiverId == receiver.Id || 
                x.SenderId == receiver.Id && x.ReceiverId == sender.Id)
        );
    }
}