using System.Net.Http;
using System.Threading.Tasks;
using InstantMessenger.Identity.Application.Features.ChangeNickname;
using InstantMessenger.Identity.Application.Features.PasswordReset;
using InstantMessenger.Identity.Application.Features.SendPasswordReset;
using InstantMessenger.Identity.Application.Features.SignIn;
using InstantMessenger.Identity.Application.Features.SignUp;
using InstantMessenger.Identity.Application.Features.VerifyUser;
using InstantMessenger.Identity.Application.Queries;
using RestEase;

namespace InstantMessenger.IntegrationTests.Api
{
    [Header("User-Agent", "RestEase")]
    public interface IIdentityApi
    {

        [Post("/api/identity/sign-up")]
        Task<HttpResponseMessage> SignUp([Body]SignUpCommand command);

        [Post("/api/identity/activate")]
        Task<HttpResponseMessage> ActivateAccount([Body]ActivateCommand command);

        [Post("/api/identity/sign-in")]
        Task<AuthDto> SignIn([Body] SignInCommand command);

        [Get("/api/identity/me")]
        Task<UserDto> Me([Header("Authorization")] string authorization);

        [Post("/api/identity/forgot-password")]
        Task<HttpResponseMessage> ForgotPassword([Body]SendPasswordResetCommand command);

        [Post("/api/identity/reset-password")]
        Task<HttpResponseMessage> ResetPassword([Body]ResetPasswordCommand command);

        [Put("/api/identity/nickname")]
        Task<HttpResponseMessage> ChangeNickname([Header("Authorization")] string token, [Body] ChangeNicknameApiRequest request);

        [Post("/api/identity/avatar")]
        Task<HttpResponseMessage> ChangeAvatar([Header("Authorization")] string token,
            [Body] HttpContent content);

    }
    public static class Extensions
    {
        public static async Task<HttpResponseMessage> ChangeAvatar(this IIdentityApi api, string token, byte[] imageData)
        {
            var content = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(imageData);
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            content.Add(imageContent, "image", "avatar.png");
            return await api.ChangeAvatar(token, content);
        }
    }
}