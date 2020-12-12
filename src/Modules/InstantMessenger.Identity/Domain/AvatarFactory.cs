using System.IO;
using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Exceptions;
using InstantMessenger.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace InstantMessenger.Identity.Domain
{
    public static class AvatarFactory
    {
        public static async Task<Avatar> CreateFrom(IFormFile file)
        {
            await using var image = new MemoryStream();
            await file.CopyToAsync(image);
            var avatar = await Avatar.Create(image.ToArray());
            if (avatar.IsFailure)
            {
                var _ = avatar.Error switch
                {
                    { } e when e == AvatarError.SizeTooBig => throw new AvatarSizeTooBigException(),
                    { } e when e == AvatarError.InvalidFormat => throw new ImageFormatNotSupportedException(),
                    _ => false
                };
            }

            return avatar.Value;
        }
    }
}