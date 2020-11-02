using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Identity.Api.Features.VerifyUser
{
    public class ActivateCommand : ICommand
    {
        public Guid UserId { get; }
        public string Token { get; }

        public ActivateCommand(Guid userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}