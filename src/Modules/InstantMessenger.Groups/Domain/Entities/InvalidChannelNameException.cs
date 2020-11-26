using InstantMessenger.Groups.Domain.Exceptions;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class InvalidChannelNameException : DomainException
    {
        public string Name { get; }
        public override string Code => "invalid_channel_name";

        public InvalidChannelNameException(string name) : base($"'{name}' is invalid channel name.")
        {
            Name = name;
        }
    }
    public class InvalidPermissionOverride : DomainException
    {
        public string Name { get; }
        public override string Code => "invalid_permission_override";

        public InvalidPermissionOverride(string name) : base($"'{name}' is invalid permission override.")
        {
            Name = name;
        }
    }
}