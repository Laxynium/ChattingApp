using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace InstantMessenger.Identity.Api.Features.ChangeAvatar
{
    public sealed class ChangeAvatarApiRequest
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}