using InstantMessenger.SharedKernel;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvalidAvatarException : DomainException
    {
        private readonly AvatarError _error;
        public override string Code => _error.Name;

        public InvalidAvatarException(AvatarError error) : base(error.Message)
        {
            _error = error;
        }
    }
}