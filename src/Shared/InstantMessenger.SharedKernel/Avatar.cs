using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using CSharpFunctionalExtensions;
using Mainwave.MimeTypes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace InstantMessenger.SharedKernel
{
    public class AvatarFile
    {
        public long Length { get; }
        public MemoryStream Data { get; }

        public AvatarFile(long length, MemoryStream data)
        {
            Length = length;
            Data = data;
        }
    }

    public class AvatarError : SmartEnum<AvatarError,string>
    {
        public string Message { get; }
        public static readonly AvatarError SizeTooBig = new AvatarError(nameof(SizeTooBig), "Avatar size is too big.");
        public static readonly AvatarError InvalidFormat = new AvatarError(nameof(InvalidFormat), "Image format is not supported.");

        private AvatarError(string name, string message):base(name,name)
        {
            Message = message;
        }
    }
    public class Avatar : SimpleValueObject<byte[]>
    {
        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { };
        }

        private Avatar(byte[] value) : base(value)
        {
        }

        public static async Task<Result<Avatar,AvatarError>> Create(AvatarFile avatarFile)
        {
            if (avatarFile.Length > MB(5))
            {
                return Result.Failure<Avatar, AvatarError>(AvatarError.SizeTooBig);
            }

            avatarFile.Data.Position = 0;
            var image = Image.Load(avatarFile.Data, out var format);
            if (!format.MimeTypes.Contains(MimeType.Image.Png))
            {
                return Result.Failure<Avatar, AvatarError>(AvatarError.InvalidFormat);
            }
            var resized = image.Clone(x => x.Resize(128, 128));
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
}