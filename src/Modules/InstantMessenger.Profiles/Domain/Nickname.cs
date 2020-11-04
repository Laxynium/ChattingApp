using CSharpFunctionalExtensions;

namespace InstantMessenger.Profiles.Domain
{
    public class Nickname : SimpleValueObject<string>
    {
        public Nickname(string value) : base(value)
        {
        }
    }
}