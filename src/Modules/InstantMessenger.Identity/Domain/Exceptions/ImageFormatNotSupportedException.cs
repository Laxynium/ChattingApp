namespace InstantMessenger.Identity.Domain.Exceptions
{
    public class ImageFormatNotSupportedException : DomainException
    {
        public override string Code => "unsupported_image_format";

        public ImageFormatNotSupportedException() : base("Image format is not supported.")
        {
        }
    }
}