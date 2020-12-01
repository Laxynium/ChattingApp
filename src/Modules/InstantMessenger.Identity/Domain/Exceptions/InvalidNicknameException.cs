namespace InstantMessenger.Identity.Domain.Exceptions
{
    public class InvalidNicknameException : DomainException
    {
        public string Nickname { get; }

        public override string Code => "invalid_nickname";

        public InvalidNicknameException(string nickname) : base($"'{nickname}' is invalid nickname.")
        {
            Nickname = nickname;
        }
    }
}