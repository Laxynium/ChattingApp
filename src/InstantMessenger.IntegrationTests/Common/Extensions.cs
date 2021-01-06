using System;
using InstantMessenger.Identity.Application.Features.SignIn;

namespace InstantMessenger.IntegrationTests.Common
{
    internal static class Extensions
    {
        public static string BearerToken(this UserDto user) 
            => $"Bearer {user.Token}";
        public static Guid UserId(this UserDto user)
            => user.UserId;

        public static string BearerToken(this AuthDto user) 
            => $"Bearer {user.Token}";

        public static Guid UserId(this AuthDto user) 
            => Guid.Parse(user.Subject);
    }
}