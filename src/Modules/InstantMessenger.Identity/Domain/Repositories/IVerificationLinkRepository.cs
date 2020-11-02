using System;
using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;

namespace InstantMessenger.Identity.Domain.Repositories
{
    public interface IVerificationLinkRepository
    {
        Task AddAsync(ActivationLink activationLink);
        Task<ActivationLink> GetAsync(Guid userId);
    }
}