using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Identity.Api.Features.ChangeNickname
{
    internal sealed class ChangeNicknameCommand : ICommand
    {
        public Guid UserId { get; }
        public string Nickname { get; }

        public ChangeNicknameCommand(Guid userId, string nickname)
        {
            UserId = userId;
            Nickname = nickname;
        }
    }
}