namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvalidInvitationCodeException : DomainException
    {
        public string Name { get; }
        public override string Code => "invalid_invitation_code";

        public InvalidInvitationCodeException(string name) : base($"'{name}' is invalid invitation code.")
        {
            Name = name;
        }
    }
}