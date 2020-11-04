using CSharpFunctionalExtensions;

namespace InstantMessenger.Profiles.Domain
{
    public class Nickname : SimpleValueObject<string>
    {
        private Nickname():base(""){}
        private Nickname(string value) : base(value)
        {
            
        }

        public static Nickname Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidNicknameException();
            }
            return new Nickname(value);
        }
    }
}