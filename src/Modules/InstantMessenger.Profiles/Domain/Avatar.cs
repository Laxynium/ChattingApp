using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Mainwave.MimeTypes;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace InstantMessenger.Profiles.Domain
{
    public class Avatar : SimpleValueObject<byte[]>
    {
        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[]{ };
        }

        private Avatar(byte[] value) : base(value)
        {
        }

        public static async Task<Avatar> Create(IFormFile imageData)
        {
            if (imageData.Length > MB(5))
            {
                throw new AvatarSizeTooBigException();
            }
            await using var imageMs = new MemoryStream();
            await imageData.CopyToAsync(imageMs);
            imageMs.Position = 0;
            var image = Image.Load(imageMs, out var format);
            if (!format.MimeTypes.Contains(MimeType.Image.Png))
            {
                throw new ImageFormatNotSupportedException();
            }
            var resized = image.Clone(x=>x.Resize(128,128));
            await using var ms = new MemoryStream();
            await resized.SaveAsPngAsync(ms);
            return new Avatar(ms.ToArray());
        }

        private static int MB(int size)
        {
            return size * 1024 * 1024;
        }

        public string AsBase64String()
        {
            return Image.Load(Value, new PngDecoder()).ToBase64String(PngFormat.Instance);
        }
    }

    internal class ImageFormatNotSupportedException : Exception
    {
    }
}