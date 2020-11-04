using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace InstantMessenger.Profiles.Api.Features.AvatarChange
{
    public sealed class ChangeAvatarApiRequest
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}