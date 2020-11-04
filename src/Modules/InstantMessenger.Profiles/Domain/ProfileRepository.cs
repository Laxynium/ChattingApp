using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Profiles.Domain
{
    internal sealed class ProfileRepository : IProfileRepository
    {
        private readonly ProfilesContext _context;

        public ProfileRepository(ProfilesContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Profile profile)
        {
            await _context.Profiles.AddAsync(profile);
        }

        public async Task<Profile> GetAsync(Guid profileId)
        {
            return await _context.Profiles.FirstOrDefaultAsync(x=>x.Id == profileId);
        }

        public async Task<bool> ExistsAsync(Guid profileId)
        {
            return await _context.Profiles.AnyAsync(x => x.Id == profileId);
        }
    }
}