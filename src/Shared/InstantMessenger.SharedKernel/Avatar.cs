﻿using System;
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
    public class AvatarError : SmartEnum<AvatarError,string>
    {
        public string Message { get; }
        public static readonly AvatarError SizeTooBig = new(nameof(SizeTooBig), "Avatar size is too big.");
        public static readonly AvatarError InvalidFormat = new(nameof(InvalidFormat), "Image format is not supported.");

        public static readonly AvatarError InvalidBase64Format = new(nameof(InvalidBase64Format),
            "Base64 string is in invalid format. Expected format is: 'data:{mime-type};base64,{data}'"); 
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

        public static async Task<Result<Avatar, AvatarError>> FromBase64String(string base64)
        {
            var parsed = Cimpress.DataUri.DataUri.Parse(base64);
            if (!parsed.Base64)
                return Result.Failure<Avatar, AvatarError>(AvatarError.InvalidBase64Format);
            if (parsed.MediaType.MimeType != PngFormat.Instance.DefaultMimeType)
                return Result.Failure<Avatar, AvatarError>(AvatarError.InvalidFormat);
            var base64Bytes = Convert.FromBase64String(parsed.Data);
            return await Create(base64Bytes);
        }
        
        public static async Task<Result<Avatar, AvatarError>> Create(byte[] data)
        {
            if(data.Length > MB(5))
            {
                return Result.Failure<Avatar, AvatarError>(AvatarError.SizeTooBig);
            }
            var image = Image.Load(data, out var format);
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

        public static string ToBase64String(byte[] data) => Image.Load(data, new PngDecoder()).ToBase64String(PngFormat.Instance);

        public string ToBase64String()
        {
            return Image.Load(Value, new PngDecoder()).ToBase64String(PngFormat.Instance);
        }
    }
}