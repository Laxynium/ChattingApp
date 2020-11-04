using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Profiles.Api.Features.NicknameChange
{
    internal sealed class ChangeNicknameCommand : ICommand
    {
        public Guid ProfileId { get; }
        public string Nickname { get; }

        public ChangeNicknameCommand(Guid profileId, string nickname)
        {
            ProfileId = profileId;
            Nickname = nickname;
        }
    }
}