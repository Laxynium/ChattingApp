using System;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.ValueObjects;

namespace InstantMessenger.Friendships.Domain.Repositories
{
    public interface IInvitationRepository
    {
        Task AddAsync(Invitation invitation);
        Task<Invitation> GetAsync(InvitationId invitationId);
    }
}