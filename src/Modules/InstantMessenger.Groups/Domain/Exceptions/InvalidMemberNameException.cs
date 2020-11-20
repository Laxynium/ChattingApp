namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvalidMemberNameException : DomainException
    {
        public string Name { get; }
        public override string Code => "invalid_member_name";

        public InvalidMemberNameException(string name) : base($"'{name}' is invalid member name.")
        {
            Name = name;
        }
    }
}