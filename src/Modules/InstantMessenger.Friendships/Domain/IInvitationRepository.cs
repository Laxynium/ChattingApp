using System;
using System.Threading.Tasks;

namespace InstantMessenger.Friendships.Domain
{
    public interface IInvitationRepository
    {
        Task AddAsync(Invitation invitation);
        Task<Invitation> GetAsync(Guid invitationId);
    }
}