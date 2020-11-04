using System.Threading.Tasks;
using InstantMessenger.Profiles;
using RestEase;

namespace InstantMessenger.IntegrationTests.Identity
{
    [Header("User-Agent", "RestEase")]
    public interface IProfilesApi
    {
        [Get("/api/profiles")]
        Task<ProfileDto> Get([Header("Authorization")] string authorization);
    }
}