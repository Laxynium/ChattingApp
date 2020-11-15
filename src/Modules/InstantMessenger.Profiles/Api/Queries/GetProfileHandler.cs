using System.Threading.Tasks;
using InstantMessenger.Profiles.Domain;
using InstantMessenger.Shared.Queries;

namespace InstantMessenger.Profiles.Api.Queries
{
    public sealed class GetProfileHandler : IQueryHandler<GetProfile, ProfileDto>
    {
        private readonly IProfileRepository _profileRepository;

        public GetProfileHandler(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        public async Task<ProfileDto> HandleAsync(GetProfile query)
        {
            var profile = await _profileRepository.GetAsync(query.ProfileId);
            if (profile is null)
                return null;

            return new ProfileDto(profile.Id, profile?.Avatar?.AsBase64String());
        }
    }
}