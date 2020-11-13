using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Identity.Api.Features.PasswordReset
{
    public class ResetPasswordCommand : ICommand
    {
        public Guid UserId { get; }
        public string Token { get; }
        public string Password { get; }

        public ResetPasswordCommand(Guid userId, string token, string password)
        {
            UserId = userId;
            Token = token;
            Password = password;
        }
    }
}