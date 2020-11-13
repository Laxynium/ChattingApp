﻿using System.Net.Http;
using System.Threading.Tasks;
using InstantMessenger.Identity.Api.Features.PasswordReset;
using InstantMessenger.Identity.Api.Features.SendPasswordReset;
using InstantMessenger.Identity.Api.Features.SignIn;
using InstantMessenger.Identity.Api.Features.SignUp;
using InstantMessenger.Identity.Api.Queries;
using RestEase;

namespace InstantMessenger.IntegrationTests.Api
{
    [Header("User-Agent", "RestEase")]
    public interface IIdentityApi
    {

        [Post("/api/identity/sign-up")]
        Task<HttpResponseMessage> SignUp([Body]SignUpCommand command);

        [Get("/api/identity/activate")]
        Task<HttpResponseMessage> ActivateAccount([Query]string userId, [Query]string token);

        [Post("/api/identity/sign-in")]
        Task<AuthDto> SignIn([Body] SignInCommand command);

        [Get("/api/identity/me")]
        Task<UserDto> Me([Header("Authorization")] string authorization);

        [Post("/api/identity/forgot-password")]
        Task<HttpResponseMessage> ForgotPassword([Body]SendPasswordResetCommand command);

        [Post("/api/identity/reset-password")]
        Task<HttpResponseMessage> ResetPassword([Body]ResetPasswordCommand command);
    }
}