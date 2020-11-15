using System.Net.Http;
using System.Threading.Tasks;
using InstantMessenger.Profiles.Api.Queries;
using RestEase;

namespace InstantMessenger.IntegrationTests.Api
{
    [Header("User-Agent", "RestEase")]
    public interface IProfilesApi
    {
        [Get("/api/profiles")]
        Task<ProfileDto> Get([Header("Authorization")] string token);

        [Post("/api/profiles/avatar")]
        Task<HttpResponseMessage> ChangeAvatar([Header("Authorization")] string token,
            [Body]HttpContent content);
    }

    public static class Extensions
    {
        public static async Task<HttpResponseMessage> ChangeAvatar(this IProfilesApi api, string token, byte[] imageData)
        {
            var content = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(imageData);
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            content.Add(imageContent, "image", "avatar.png");
            return await api.ChangeAvatar($"Bearer {token}", content);
        }
    }

}