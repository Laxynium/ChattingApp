using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class ChannelName : SimpleValueObject<string>
    {
        private ChannelName():base(default){}

        private ChannelName(string value) : base(value)
        {
        }

        public static ChannelName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidChannelNameException(value);
            return new ChannelName(value);
        }
    }
}