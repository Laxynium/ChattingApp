using InstantMessenger.Identity.Domain.Exceptions;

namespace InstantMessenger.Identity.Domain.Entities
{
    public class ImageFormatNotSupportedException : DomainException
    {
        public override string Code => "unsupported_image_format";

        public ImageFormatNotSupportedException() : base("Image format is not supported.")
        {
        }
    }
}