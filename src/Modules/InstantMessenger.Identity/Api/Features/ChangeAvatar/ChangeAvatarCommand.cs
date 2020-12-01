using System;
using InstantMessenger.Shared.Commands;
using Microsoft.AspNetCore.Http;

namespace InstantMessenger.Identity.Api.Features.ChangeAvatar
{
    internal sealed class ChangeAvatarCommand : ICommand
    {
        public Guid UserId { get; }
        public IFormFile Image { get; }

        public ChangeAvatarCommand(Guid userId, IFormFile image)
        {
            UserId = userId;
            Image = image;
        }
    }
}