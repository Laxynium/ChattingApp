using System;
using System.Threading.Tasks;

namespace InstantMessenger.Profiles.Domain
{
    public interface IProfileRepository
    {
        Task AddAsync(Profile profile);
        Task<Profile> GetAsync(Guid profileId);
        Task<bool> ExistsAsync(Guid profileId);
    }
}