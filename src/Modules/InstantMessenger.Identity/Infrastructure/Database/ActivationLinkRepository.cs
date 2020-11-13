using System;
using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Identity.Infrastructure.Database
{
    internal sealed class ActivationLinkRepository : IActivationLinkRepository
    {
        private readonly IdentityContext _context;

        public ActivationLinkRepository(IdentityContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ActivationLink activationLink)
        {
            await _context.AddAsync(activationLink);
        }

        public async Task<ActivationLink> GetAsync(Guid userId)
        {
            return await _context.VerificationLinks.SingleOrDefaultAsync(x => x.UserId == userId);
        }
    }
}