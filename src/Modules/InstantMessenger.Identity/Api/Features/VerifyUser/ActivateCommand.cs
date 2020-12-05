using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Identity.Api.Features.VerifyUser
{
    public class ActivateCommand : ICommand
    {
        public Guid UserId { get; }
        public string Token { get; }
        public string Nickname { get; }

        public ActivateCommand(Guid userId, string token, string nickname)
        {
            UserId = userId;
            Token = token;
            Nickname = nickname;
        }
    }
}