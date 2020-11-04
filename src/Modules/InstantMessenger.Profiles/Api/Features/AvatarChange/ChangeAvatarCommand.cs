using System;
using InstantMessenger.Shared.Commands;
using Microsoft.AspNetCore.Http;

namespace InstantMessenger.Profiles.Api.Features.AvatarChange
{
    internal sealed class ChangeAvatarCommand : ICommand
    {
        public Guid ProfileId { get; }
        public IFormFile Image { get; }

        public ChangeAvatarCommand(Guid profileId, IFormFile image)
        {
            ProfileId = profileId;
            Image = image;
        }
    }
}