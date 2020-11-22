using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;

namespace InstantMessenger.Groups.Domain.Repositories
{
    public interface IInvitationRepository
    {
        public Task AddAsync(Invitation invitation);
        Task<Invitation> GetAsync(InvitationCode invitationCode);
    }
}