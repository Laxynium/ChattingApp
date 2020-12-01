using System;

namespace InstantMessenger.Profiles.Domain.Exceptions
{
    public class AvatarSizeTooBigException : DomainException
    {
        public override string Code => "avatar_size_too_big";

        public AvatarSizeTooBigException() : base("Avatar file size is too big")
        {
        }
    }
}